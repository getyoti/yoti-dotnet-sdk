# .NET 4.7 Example project (Windows only)

## Instructions
1) Clone this repo
1) Open the Yoti.Auth.sln solution in Visual Studio, found in the [/src](../../../../src) folder
1) Rename the [secrets.config.example](../../../src/Examples/Profile/47Example/secrets.config.example) file to `secrets.config`
1) Fill in the environment variables in this file with the ones specific to your application (mentioned in the [Client initialisation](#client-initialisation) section)
1) From the Yoti Hub, set the application domain to `localhost:44321` and the scenario callback URL to `/account/connect`
1) Right click on "47Example" in the Solution Explorer and select "Set as StartUp Project"
1) Run the project
1) The web page should open automatically with URL `https://localhost:44321`
