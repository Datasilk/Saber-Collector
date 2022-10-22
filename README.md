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

#### Then, for Saber Visual Studio Users
While using the latest source code for Saber, do the following:
* Execute `git clone https://github.com/Datasilk/Saber-Collector Collector` within the folder `/App/Vendors/`


#### Or, for Saber DevOps Users
While using the latest release of Saber, do the following:
* Download latest release of [Saber.Vendors.Collector](https://github.com/Datasilk/Saber-Collector/releases)
* Extract all files & folders from either the `win-x64` or `linux-x64` zip folder to Saber's `/Vendors/` folder

### Publish
* run command `./publish.bat`
* publish `bin/Publish/Collector.7z` as latest release

### config.json

```json
{
  "browser": {
    "endpoint": {
      "development": "http://localhost:7007/GetDOM",
      "staging": "http://localhost:7007/GetDOM",
      "production": "http://localhost:7007/GetDOM"
    }
  },
  "storage": {
    "development": "/Content/Collector/",
    "staging": "/Content/Collector/",
    "production": "/Content/Collector/"
  },
  "domains": {
    "downloads": {
      "minIntervals": 60
    }
  }
}
```

#### browser.endpoint.{environment}
The URL for your instance of [Charlotte's Web](https://github.com/Datasilk/Charlottes-Web), a load balancer application that
delegates requests to a cluster of [Charlotte](https://github.com/Datasilk/Charlotte) workers.

#### storage.{environment}
The relative or absolute path to the folder where you'd like to store downloaded content for Collector. 
This path should typically be located on a network drive where instances of Collector running on multiple machines can access the drive in a local network.
Also note that the path must end with a `/` slash.

#### domains.downloads.minIntervals
This number is used to make sure that Collector doesn't make too many requests on any given domain in a short period of time. The value is in seconds and 
determines the minimum time between each request made to a single domain. Collector will exclude any download queue items that meet this criteria when
finding the next item in queue.