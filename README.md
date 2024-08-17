Instructions on how to use the application:

1. Please specify the ApiAuthToken and the User-Agent field in the RedditConsoleApplication/appsettings.json file 
User-Agent should be following
<platform>:<app ID>:<version string> (by /u/<reddit username>)
Example:
User-Agent: android:com.example.myredditapp:v1.2.3 (by /u/kemitche)

appsettings.json:
{
  "AppConfiguration": {
    "UserAccount": {
      "ApiAuthToken": “input the reddit API Token without the 'bearer'",
      "UserAgent": “Specify the user-agent as described above"
    }
  }
}
