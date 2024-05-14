# .NET Core Example project 

## 1) Setup
1) Clone this repo
1) Navigate to this folder
1) Rename the [.env.example](.env.example) file to `.env`
1) Fill in the environment variables in this file with the ones specific to your application, generated in the [Yoti Hub](https://hub.yoti.com) when you create (and then publish) your application

## 2a) Running With Docker
1) From the Yoti Hub, set the application domain to `localhost:44380`
1) `docker-compose build --no-cache`
1) `docker-compose up`
1) Navigate to <https://localhost:44380/generate-share>

>If you encounter a "permission denied" error when trying to access the mounted .pem file, try disabling and re-enabling your shared drive in Docker settings.

## 2b) Running With .NET Core installed locally
1) From the Yoti Hub, set the application domain to `localhost:44344` 
1) Download the .NET SDK for your operating system from step no.1 on ([Windows](https://www.microsoft.com/net/learn/get-started/windows) | [Linux](https://www.microsoft.com/net/learn/get-started/linux/rhel) | [MacOS](https://www.microsoft.com/net/learn/get-started/macos))
1) Enter `dotnet run -p DigitalIdentityExample.csproj` into the terminal 
1) Navigate to the page specified in the terminal window, which should be <https://localhost:44344/generate-share>
