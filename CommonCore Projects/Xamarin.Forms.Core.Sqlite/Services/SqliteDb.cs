using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SQLite;

namespace Xamarin.Forms.Core
{

    public class SqliteDb : ISqliteDb
	{
		protected SQLiteAsyncConnection conn;

        private List<string> tableRegistry;
        private static readonly AsyncLock Mutex = new AsyncLock();
        private Dictionary<Type, PropertyInfo[]> encrytedProperties;

        private async Task ValidateSetup<T>() where T : ICoreSqlModel, new()
        {
            await Task.Run(async() => { 
               
                var fullName = typeof(T).FullName;
                if(conn==null)
                {
                    string folder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                    conn = new SQLiteAsyncConnection(System.IO.Path.Combine(folder, CoreSettings.Config.SqliteSettings.SQLiteDatabase));
                    encrytedProperties = new Dictionary<Type, PropertyInfo[]>();
                }
                if(tableRegistry==null)
                {
                    tableRegistry = new List<string>();
                }

                if (tableRegistry.Any(x => x == fullName))
                {
                    return;
                }
                else{
                    try
                    {
                        await conn.CreateTableAsync<T>();
                        var t = typeof(T);
                        encrytedProperties.Add(t, GetEncryptePropertyList(t));
                        tableRegistry.Add(fullName);
                    }
                    catch (Exception ex)
                    {
                        ex.ConsoleWrite(true);
                    }

                }
            });

        }

        private PropertyInfo[] GetEncryptePropertyList(Type type)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => p.GetCustomAttributes(typeof(EncryptedPropertyAttribute)).Count() > 0).ToArray();
            return props;
        }

        public async Task<(List<T> Response, bool Success, Exception Error)> GetAll<T>() where T : ICoreSqlModel, new()
		{
			try
			{
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await ValidateSetup<T>();
					var query = conn.Table<T>();
					var response = await query.ToListAsync();
                    encrytedProperties.UnEncryptedDataModelProperties<T>(response);
                    return (response, true, null);
                }
			}
			catch (Exception ex)
			{
				return (null, false, ex);
			}

		}

		public async Task<(bool Success, Exception Error)> TruncateAsync<T>() where T : ICoreSqlModel, new()
		{
            (bool Success, Exception Error) response = (false, null);
			await conn.RunInTransactionAsync(async (tran) =>
			{
				try
				{
                    using (await Mutex.LockAsync().ConfigureAwait(false))
                    {
                        await ValidateSetup<T>();

                        await conn.DropTableAsync<T>();
                        await conn.CreateTableAsync<T>();
                        tran.Commit();
                        response.Success = true;
                    }
				}
				catch (Exception ex)
				{
                    response.Error = ex;
					tran.Rollback();
				}

			});
			return response;
		}

        public async Task<(T Response, bool Success, Exception Error)> GetByCorrelationID<T>(Guid CorrelationID) where T : class, ICoreSqlModel, new()
		{
			try
			{
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await ValidateSetup<T>();
                    var query = conn.Table<T>().Where(x => x.CorrelationID == CorrelationID);
                    var response = await query.FirstOrDefaultAsync();
                    encrytedProperties.UnEncryptedDataModelProperties<T>(response);
                    return (response, true, null);
                }
			}
			catch (Exception ex)
			{
                return (null, false, ex);
			}

		}
        public async Task<(List<T> Response, bool Success, Exception Error)> GetByQuery<T>(Expression<Func<T, bool>> exp) where T : ICoreSqlModel, new()
		{
			try
			{
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await ValidateSetup<T>();
                    var query = conn.Table<T>().Where(exp);
                    var response = await query.ToListAsync();
                    encrytedProperties.UnEncryptedDataModelProperties<T>(response);
                    return (response, true, null);
                }
			}
			catch (Exception ex)
			{
				return (null, false, ex);
			}

		}

		public async Task<(bool Success, Exception Error)> AddOrUpdate<T>(IEnumerable<T> collection) where T : ICoreSqlModel, new()
		{
			try
			{
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await ValidateSetup<T>();

                    encrytedProperties.EncryptedDataModelProperties<T>(collection);

                    var inserts = new List<T>();
                    var updates = new List<T>();
                    foreach (var obj in collection)
                    {
                        obj.UTCTickStamp = DateTime.UtcNow.Ticks;
                        if (obj.CorrelationID != default(Guid))
                        {
                            var exists = await RecordExists<T>(obj.CorrelationID);
                            if(exists)
                                updates.Add(obj);
                            else
                                inserts.Add(obj);
                        }
                        else
                        {
                            obj.CorrelationID = Guid.NewGuid();
                            inserts.Add(obj);
                        }

                    }

                    var totalInserted = await conn.InsertAllAsync(inserts);
                    var totalUpdated = await conn.UpdateAllAsync(updates);

                    bool result = false;
                    if (totalUpdated == updates.Count && totalInserted == inserts.Count)
                        result = true;

                    return (result, null);
                }
			}
			catch (Exception ex)
			{
				return (false, ex);
			}
		}

		public async Task<(bool Success, Exception Error)> AddOrUpdate<T>(T obj) where T : ICoreSqlModel, new()
		{
			int rowsAffected = 0;
			obj.UTCTickStamp = DateTime.UtcNow.Ticks;

			try
			{
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await ValidateSetup<T>();

                    encrytedProperties.EncryptedDataModelProperties<T>(obj);

                    if (obj.CorrelationID != default(Guid))
                    {
                        var exists = await RecordExists<T>(obj.CorrelationID);
                        if (exists)
                            rowsAffected = await conn.UpdateAsync(obj);
                        else
                            rowsAffected = await conn.InsertAsync(obj);
                        
                    }
                    else
                    {
                        obj.CorrelationID = Guid.NewGuid();
                        rowsAffected = await conn.InsertAsync(obj);
                        if (rowsAffected != 1)
                            obj.CorrelationID = default(Guid);
                    }

                    var result = rowsAffected == 1 ? true : false;
                    return (result, null);
                }
			}
			catch (Exception ex)
			{
				return (false, ex);
			}

		}

        public async Task<(bool Success, Exception Error)> DeleteByCorrelationID<T>(Guid correlationId, bool softDelete = false) where T : class, ICoreSqlModel, new()
		{

			try
			{
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    var result = false;
                    await ValidateSetup<T>();

                    var obj = await conn.Table<T>().Where(x => x.CorrelationID == correlationId).FirstOrDefaultAsync();
                    int rowsAffected = 0;
                    if (obj!=null)
                    {
                        if (softDelete)
                        {
                            obj.UTCTickStamp = DateTime.UtcNow.Ticks;
                            obj.MarkedForDelete = true;
                            rowsAffected = await conn.UpdateAsync(obj);
                        }
                        else
                        {
                            rowsAffected = await conn.DeleteAsync(obj);
                        }
                        result = rowsAffected == 1 ? true : false;
                    }
                    return (result, null);
                }
			}
			catch (Exception ex)
			{
				return (false, ex);
			}
		}
		public async Task<(bool Success, Exception Error)> DeleteByQuery<T>(Expression<Func<T, bool>> exp, bool softDelete = false) where T : ICoreSqlModel, new()
		{
			try
			{
                using (await Mutex.LockAsync().ConfigureAwait(false))
                {
                    await ValidateSetup<T>();
					var result = false;
                    int rowsAffected = 0;
                    var obj = await conn.Table<T>().Where(exp).FirstOrDefaultAsync();
                    if (obj != null)
                    {
                        if (softDelete)
                        {
                            obj.UTCTickStamp = DateTime.UtcNow.Ticks;
                            obj.MarkedForDelete = true;
                            rowsAffected = await conn.UpdateAsync(obj);
                        }
                        else
                        {
                            rowsAffected = await conn.DeleteAsync(obj);
                        }
                        result = rowsAffected == 1 ? true : false;
                    }
                    return (result, null);
                }
			}
			catch (Exception ex)
			{
			    return (false, ex);
			}
		}

        private async Task<bool> RecordExists<T>(Guid correlationId) where T : ICoreSqlModel, new()
        {
            var query = $"SELECT CorrelationID FROM {typeof(T).Name} WHERE CorrelationID = '{correlationId.ToString()}'";
            var lst = await conn.QueryAsync<List<Guid>>(query);
            return lst.Count() == 0 ? false : true;
        }

	}

}

