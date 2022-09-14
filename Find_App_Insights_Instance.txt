function Install-Extension([string]$extensionName, $installedExtensions){
    [bool] $isExtensionInstalled = 0;
    foreach($i in $installedExtensions){
        if ($i.name -eq $extensionName){
            $isExtensionInstalled = 1;
            break;
        }
    }
 
    if ($isExtensionInstalled -eq 0){
        Write-Output "Started installing extension $($extensionName)";
        az extension add --name $extensionName --output none;   
        Write-Output "Finished installing extension $($extensionName)";
    }
    else{
        Write-Output "Extension $($extensionName) is already installed.";
    }
}
 
$installedExtensions = az extension list --output json | ConvertFrom-Json
Install-Extension "account" $installedExtensions
Install-Extension "application-insights" $installedExtensions
 
Write-Output "Opening browser window... Please login to your Azure account."
az login | Out-Null

$username = az account show --query user.name --output tsv;
$subscriptions = az account subscription list --query "[*].[subscriptionId,displayName]" --output json | ConvertFrom-Json;
$totalSubscriptions = $subscriptions.Length;
$counter = 1;
Write-Output "Successfully retrieved $($totalSubscriptions) subscriptions for user - $($username)";
 
while($true)
{
    $instrumentationKey = Read-Host "Enter App Insights instrumentation key ";
    $Result = [System.Guid]::empty;
    [bool] $isValid = [System.Guid]::TryParse($instrumentationKey,[System.Management.Automation.PSReference]$Result)
    if ($isValid -eq 1)
    {
        break;
    }
    else
    {
        Write-Output "Invalid input. Instrumentation key must be a guid."
    }
}
 
foreach($i in $subscriptions)
{    
    az account set --subscription $i.GetValue(0);
    $url = "https://management.azure.com/subscriptions/$($i.GetValue(0))/providers/Microsoft.Insights/components?api-version=2015-05-01";
    $token = az account get-access-token --output json | ConvertFrom-Json;
    $response = Invoke-RestMethod $url -Headers @{"Authorization"="Bearer $($token.accessToken)"};

    Write-Output "Searching for key - $($instrumentationKey) among $($response.value.Length) App Insights instances in subscription $($counter)/$($totalSubscriptions) [$($i.GetValue(1))]";
    foreach($j in $response.value)
    {
        if ($j.properties.InstrumentationKey -eq $instrumentationKey){
            Write-Output "Found app insights instance $($j.name) in subscription $($i.GetValue(1).ToLower())";
            $url = "https://portal.azure.com/#@XYZ.COM.PQ/resource/$($j.id)/overview";
            [System.Diagnostics.Process]::Start("C:\Program Files\Google\Chrome\Application\chrome.exe", $url);
            Read-Host "Press Enter to terminate the script"
            Exit
        }
    }
    ++$counter;
}
 
Write-Output "Instrumentation key not found in any of the subscriptions that your account - $($username) has access to.";
Read-Host "Press Enter to terminate the script"
Exit
