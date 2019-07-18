using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;
using System.Linq;
using System.Linq.Expressions;

namespace Xamarin.Forms.Core
{
    public class LiteNoSql: ILiteNoSql
    {
        private SemaphoreSlim semaphore;
        private string filePath;
        public LiteDatabase db;

        public LiteNoSql()
        {
            semaphore = new SemaphoreSlim(1, 1);
            var fileName = CoreSettings.Config.LiteliteSettings.LiteDatabase;
            filePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + $"/{fileName}";
            db = new LiteDatabase(filePath);
        }

        public async Task<(List<T> Response, Exception Error)> GetAll<T>() where T : LiteDbModel, new()
        {
            (List<T> Response, Exception Error) response = (null, null);
            await semaphore.WaitAsync();
            try
            {
                return await Task.Run(() =>
                {
                    response.Response = db.GetCollection<T>(typeof(T).Name).FindAll().ToList<T>();
                    return response;
                });
            }
			catch (Exception ex)
			{
                response.Error = ex;
                return response;
			}
            finally{
                semaphore.Release();
            }
        }


        public async Task<(T Response, Exception Error)> GetById<T>(string _objectId) where T : LiteDbModel, new()
        {
            (T Response, Exception Error) response = (null, null);
            await semaphore.WaitAsync();
            try
            {
                return await Task.Run(() =>
                {
                    var collection = db.GetCollection<T>(typeof(T).Name);
                    var item = collection.FindById(new BsonValue(_objectId));
                    response.Response = item;
                    if (item == null)
                        response.Error = new ApplicationException("Item not found");

                    return response;

                });
            }
            catch (Exception ex)
            {
                response.Error = ex;
                return response;
            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<(List<T> Response, Exception Error)> Get<T>(Expression<Func<T, bool>> exp) where T : LiteDbModel, new()
		{
            (List<T> Response, Exception Error) response = (null, null);

			await semaphore.WaitAsync();
			try
			{
				return await Task.Run(() =>
				{
                    var collection = db.GetCollection<T>(typeof(T).Name);
                    response.Response = collection.Find(exp).ToList<T>();
					return response;
				});
			}
			catch (Exception ex)
			{
				response.Error = ex;
				return response;
			}
			finally
			{
				semaphore.Release();
			}
		}

		public async Task<(bool Success, Exception Error)> Insert<T>(T obj) where T : LiteDbModel, new()
		{
            (bool Success, Exception Error) response = (false, null);

			await semaphore.WaitAsync();
			try
			{
                return await Task.Run(() =>
                {
                    var collection = db.GetCollection<T>(typeof(T).Name);
                    if (string.IsNullOrEmpty(obj.Id))
                    {
                        obj.Id = ObjectId.NewObjectId().ToString();
                     
                    }
                    else 
                    {
                        var alreadyExists = collection.FindById(new BsonValue(obj.Id));
                        if (alreadyExists!=null) 
                        {
                            response.Error = new ApplicationException("Iteam already exists in local database");
                            return response;
                        }

                    }

                    var result = collection.Insert(obj);
					response.Success = true;
					return response;
                });
			}
            catch(Exception ex)
            {
				response.Error = ex;
				return response;
            }
			finally
			{
				semaphore.Release();
			}
		}

		public async Task<(bool Success, Exception Error)> Update<T>(T obj) where T : LiteDbModel, new()
		{
			(bool Success, Exception Error) response = (false, null);

			await semaphore.WaitAsync();
			try
			{
				return await Task.Run(() =>
				{
					var collection = db.GetCollection<T>(typeof(T).Name);
                    response.Success = collection.Update(obj);
					return response;
				});
			}
			catch (Exception ex)
			{
				response.Error = ex;
				return response;
			}
			finally
			{
				semaphore.Release();
			}
		}

		public async Task<(bool Success, Exception Error)> Delete<T>(T obj) where T : LiteDbModel, new()
		{
			(bool Success, Exception Error) response = (false, null);

			await semaphore.WaitAsync();
			try
			{
                return await Task.Run(() =>
                {
                    var collection = db.GetCollection<T>(typeof(T).Name);
                    var result = collection.Delete(x => x.Id == obj.Id);
					response.Success = result > 0 ? true : false;
                    return response;
                });
			}
			catch (Exception ex)
			{
				response.Error = ex;
				return response;
			}
			finally
			{
				semaphore.Release();
			}
		}

        public async Task BulkSync<T>(List<T> list) where T : LiteDbModel, new()
        {
            await semaphore.WaitAsync();

            await Task.Run(() =>
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                foreach (var obj in list)
                {
                    var alreadyExists = collection.FindById(new BsonValue(obj.Id));
                    if (alreadyExists != null)
                    {
                        collection.Update(obj);
                    }
                    else
                    {
                        collection.Insert(obj);
                    }
                }


            });
            semaphore.Release();
        }

        public async Task<(bool Success, Exception Error)> Delete<T>(string Id) where T : LiteDbModel, new()
		{
			(bool Success, Exception Error) response = (false, null);

			await semaphore.WaitAsync();
			try
			{
				return await Task.Run(() =>
				{
					var collection = db.GetCollection<T>(typeof(T).Name);
					var result = collection.Delete(x => x.Id == Id);
  
                    response.Success = result > 0 ? true : false;
					return response;
				});
			}
			catch (Exception ex)
			{
				response.Error = ex;
				return response;
			}
			finally
			{
				semaphore.Release();
			}
		}
    }

}
