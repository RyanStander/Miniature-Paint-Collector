using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

namespace GooglePlayGames
{
    public static class PlayGamesManager
    {
        public static void SignIn()
        {
            //PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }

        internal static void ProcessAuthentication(SignInStatus status)
        {
            if (status == SignInStatus.Success)
            {
                // Continue with Play Games Services

                string name = PlayGamesPlatform.Instance.GetUserDisplayName();
                string id = PlayGamesPlatform.Instance.GetUserId();
                string imgurl = PlayGamesPlatform.Instance.GetUserImageUrl();
            }
            else
            {
                // Disable your integration with Play Games Services or show a login button
                // to ask users to sign-in. Clicking it should call
                // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
            }
        }
    }
}
