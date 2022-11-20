cd ../../
dotnet publish -c release -r win-x64
#dotnet publish -c release -r linux-x64
cd bin
rmdir release /s /q
cd Vendors/Collector
gulp publish
