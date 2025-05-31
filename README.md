# DownloadSorterService

##### Download Sorter Service is a windows service which will watch over the downloads foler and automatically sort the files into folder based on the format of the files

the service default install folder contains appsettings.json which has default configurations for the mappings of the file extensions to folders.

```
"DownloadSorter": {
    ...
    "FileCategories": {
      "Images": [ ".jpg", ".jpeg", ".png", ".gif" ],
      "Videos": [ ".mp4", ".mkv", ".avi" ],
      "Documents": [ ".pdf", ".docx", ".txt" ],
      "Archives": [ ".zip", ".rar" ]
    },
    "ExcludedExtensions": [ ".tmp", ".crdownload", ".part" ]
  }
```

This can also be changes based on your preference and you can also add more

for example:

```
"DownloadSorter": {
    ...
    "FileCategories": {
      ...
      "Archives": [ ".zip", ".rar" ],
      "Audio": [".mp3",".wav"]
    },
    "ExcludedExtensions": [ ".tmp", ".crdownload", ".part" ]
  }
```

any unrecognized extensions will me moved to a misc folder. Few extensions can also be excluded from being moved (really needed to prevent the service to move the incomplete downloading files from browser you use like chrome, edge etc. )

this service can also be used in other target directories by changing the DownloadPath to some other folder.

###### NOTE: changes in appsettings.json requires service restart from windows services you can do this via the windows services GUI or using the commands:

##### cmd:

```
sc stop DownloadSorterService
sc start DownloadSorterService
```

#### How to Build and Run:

- Before we do anything make sure the downloads path in the appsettings.json is correct by default is set to my downloads folder path

```
    ...
  "DownloadSorter": {
    "DownloadPath": "C:\\Users\\abhay\\Downloads",
    ...

```

- Run a release buld and let visual studio resolve the dependencies in the project
- Once release build is complete in the project directory you publish the build to your desired location by running the command below, you can change the path to this published location

```
dotnet publish -c Release -o "C:\Program Files (x86)\DownloadSorterService"
```

- After running the command you will see the folder with all the released files in there
- We will now create new service using the command:

```
sc create DownloadSorterService binPath= "C:\Program Files (x86)\DownloadSorterService\DownloadSorterService.exe"
```

- Once the service is create we can now start this service with command:

```
sc start DownloadSorterService
```

###### this service will now start and you can test it by copy pasting a new loose file into the path
