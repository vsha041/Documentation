### Authenticating to Azure DevOps Nuget Feed

### PROBLEM
-Building from command line fails as it cannot connect to Azure DevOps feed.

-Visual Studio build fails as Visual Studio is not able to connect to Azure Dev Ops feed.

### SOLUTION
1. Navigate to https://dev.azure.com/your_organization/_usersSettings/tokens
2. Click on “New Token” link
3. Under scopes, select everything and then click Create and Copy the token. Note that you will never be able to see the token again.
4. Download the latest Nuget executable from Nuget website
5. Run the following command through admin cmd prompt.

`nuget source Add -Name "SomeName" -Source "https://pkgs.dev.azure.com/your_organization/_packaging/your_organization_packages/nuget/v3/index.json" -UserName firstName.lastName@your_organization.co.nz -Password PUT_THE_TOKEN`

Note that your Nuget.config file located at ***%appdata%/nuget*** would have been modified.
