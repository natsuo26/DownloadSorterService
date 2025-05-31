# **DownloadSorterService**

**DownloadSorterService** is a Windows service that monitors the **Downloads** folder (or any configured directory) and automatically sorts files into folders based on their file extensions.

The service installation directory includes an `appsettings.json` file that contains the default configuration for mapping file extensions to their respective categories.

### 🔧 Default Configuration Example:

```json
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

You can customize these mappings and add more categories as needed.
For example, to include audio files:

```json
"DownloadSorter": {
  ...
  "FileCategories": {
    ...
    "Archives": [ ".zip", ".rar" ],
    "Audio": [ ".mp3", ".wav" ]
  },
  "ExcludedExtensions": [ ".tmp", ".crdownload", ".part" ]
}
```

### 📁 Behavior

- Files are sorted into subfolders based on the configured categories.
- Files with extensions not recognized in any category will be moved to a `Misc` folder.
- Extensions listed in `ExcludedExtensions` (e.g., `.tmp`, `.crdownload`) are ignored—this prevents the service from moving incomplete or in-progress downloads (e.g., from browsers like Chrome or Edge).
- You can set the `DownloadPath` to target a different directory if needed.

---

### 🔁 Restarting the Service After Changes

**Important:** Any changes made to the `appsettings.json` file require a restart of the service to take effect. You can restart the service using the Windows Services GUI or via the command prompt:

```cmd
sc stop DownloadSorterService
sc start DownloadSorterService
```

---

## 🛠️ How to Build and Run

1. **Check the download path** in `appsettings.json`. By default, it is set to your Downloads folder:

```json
"DownloadSorter": {
  "DownloadPath": "C:\\Users\\abhay\\Downloads",
  ...
}
```

2. **Build the service in Release mode** using Visual Studio. Ensure that all dependencies are properly restored.

3. **Publish the build** to your desired directory using the following command (you may change the output path as needed):

```cmd
dotnet publish -c Release -o "C:\Program Files (x86)\DownloadSorterService"
```

This will generate the necessary executable and supporting files in the specified directory.

4. **Create the Windows service** using:

```cmd
sc create DownloadSorterService binPath= "C:\Program Files (x86)\DownloadSorterService\DownloadSorterService.exe"
```

5. **Start the service** with:

```cmd
sc start DownloadSorterService
```

---

Once started, the service will begin monitoring the configured download path. You can test its functionality by copying a file with a known extension into the target directory and observing it being sorted into the appropriate folder.

---
