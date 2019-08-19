Android:
    Add the following in the AndroidManifest inbetween application tags
            <provider
              android:name="android.support.v4.content.FileProvider"
              android:authorities="${applicationId}.fileprovider"
              android:exported="false"
              android:grantUriPermissions="true">
              <meta-data
                android:name="android.support.FILE_PROVIDER_PATHS"
                android:resource="@xml/provider_paths"/>
            </provider>

   Create a xml folder in resources and a file called provider_paths.xml
           <?xml version="1.0" encoding="utf-8"?>
             <paths xmlns:android="http://schemas.android.com/apk/res/android">
             <external-path name="external_files" path="." />
             <files-path name="files" path="." />
             <internal-path name="internal_files" path="." />
          </paths>