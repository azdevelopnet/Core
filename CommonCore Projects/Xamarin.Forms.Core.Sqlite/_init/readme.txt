
Required Nuget Installs
 - sqlite-net-pcl

Step 1:
    Add the following settings to the json configuration
      "SqliteSettings": {
        "SQLiteDatabase": "app.db3"
      },

Step 2: Objects saved in the database must inherit from CoreSqlModel or implement ICoreSqlModel
