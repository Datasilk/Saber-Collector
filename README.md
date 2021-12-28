# Collector
A vendor plugin for [Saber](https://saber.datasilk.io) that allows webmasters to scrape & archive content from the web & RSS feeds.

### Prerequisites
* [Saber](https://saber.datasilk.io) ([latest release](https://github.com/Datasilk/Saber/releases))

### Installation
#### First, install Charlotte
Clone the repository anywhere on your web server outside of Saber
* `git clone https://github.com/Datasilk/Charlotte`
* Open solution `Charlotte.sln` using Visual Studio 2019 or newer & build Charlotte
* execute `bin\x64\Debug\Charlotte.exe -register` in PowerShell to register the  [Charlotte](https://github.com/Datasilk/Charlotte) console application as a Windows Service, which will automatically start the WCF Hosted Service

#### For Saber Visual Studio Users
While using the latest source code for Saber, do the following:
* Execute `git clone https://github.com/Datasilk/Saber-Collector Collector` within the folder `/App/Vendors/`


#### For Saber DevOps Users
While using the latest release of Saber, do the following:
* Download latest release of [Saber.Vendors.Collector](https://github.com/Datasilk/Saber-Collector/releases)
* Extract all files & folders from either the `win-x64` or `linux-x64` zip folder to Saber's `/Vendors/` folder

#### Final Steps
* Open `App/Vendors/Collector/config.json` and set **browserPath** to point to the path to `Charlotte.exe` wherever you installed Charlotte

### Publish
* run command `./publish.bat`
* publish `bin/Publish/Collector.7z` as latest release