//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;
//using Firebase.Auth;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames.BasicApi.SavedGame;
//using UnityEngine;
//using UnityEngine.UI;
//using GooglePlayGames.BasicApi.SavedGame;

//public class LoginMain : MonoBehaviour
//{
//    public Button btnDiscord;
//    public Button btnTMI;

//    private void Start()
//    {
//        this.Init();
//    }

//    public void Init()
//    {
//        //이벤트 등록
//        GPGSManager.instance.onAuthenticate = (success) =>
//        {
//            Debug.LogFormat("onAuthenticate: {0}", success);

//            PlayGamesPlatform.Instance.RequestServerSideAccess(true, (token) =>
//            {

//                Debug.Log("********* token *********");
//                Debug.Log(token);
//                Debug.Log("*************************");

//                FirebaseAuth auth = FirebaseAuth.DefaultInstance;

//                Credential credential = PlayGamesAuthProvider.GetCredential(token);
//                auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
//                {
//                    if (task.IsCanceled)
//                    {
//                        Debug.LogFormat("******** task.IsCanceled ********");
//                        return;
//                    }

//                    if (task.IsFaulted)
//                    {
//                        Debug.LogFormat("******** task.IsFaulted ********");
//                        return;
//                    }

//                    FirebaseUser newUser = task.Result;
//                    if (newUser != null)
//                    {
//                        Debug.LogFormat("[newUser] DisplayName: {0}, UserId: {1}", newUser.DisplayName, newUser.UserId);
//                    }
//                    else
//                    {
//                        Debug.Log("newUser is null");
//                    }


//                    FirebaseUser currentUser = auth.CurrentUser;

//                    if (currentUser != null)
//                    {
//                        Debug.LogFormat("[currentUser] DisplayName: {0}, UserId: {1}", currentUser.DisplayName, currentUser.UserId);
//                    }
//                    else
//                    {
//                        Debug.LogFormat("currentUser is null");
//                    }
//                });

//            });

//        };
//        GPGSManager.instance.onSavedDataToCloud = (success) =>
//        {
//            Debug.LogFormat("onSavedDataToCloud: {0}", success);
//        };
//        GPGSManager.instance.onLoadDataFromCloud = (data) =>
//        {
//            Debug.LogFormat("onLoadDataFromCloud: {0}", data);
//        };
//        GPGSManager.instance.onErrorHandler = (error) =>
//        {
//            Debug.LogFormat(error.ToString());
//        };

//        //인증 
//        GPGSManager.instance.Authenticate();

//        //클라우드 저장 
//        this.btnDiscord.onClick.AddListener(() =>
//        {

//            Debug.LogFormat("IsAuthenticate: {0}", GPGSManager.instance.IsAuthenticate());

//            //Parameter
//            Firebase.Analytics.FirebaseAnalytics.LogEvent(GameEnums.eAnalyticsEventType.save_to_cloud.ToString());
//            GPGSManager.instance.SaveDataToCloud("Hello World!");
//        });

//        //클라우드 불러오기  
//        this.btnTMI.onClick.AddListener(() =>
//        {

//            Debug.LogFormat("IsAuthenticate: {0}", GPGSManager.instance.IsAuthenticate());
//            Firebase.Analytics.FirebaseAnalytics.LogEvent(GameEnums.eAnalyticsEventType.load_from_cloud.ToString());
//            GPGSManager.instance.LoadDataFromCloud();
//        });
//    }
//}