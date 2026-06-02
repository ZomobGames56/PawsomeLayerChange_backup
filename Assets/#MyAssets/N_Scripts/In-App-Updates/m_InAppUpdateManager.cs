using Google.Play.AppUpdate;
using Google.Play.Common;
using System.Collections;
using UnityEngine;

public class m_InAppUpdateManager : MonoBehaviour
{
    private AppUpdateManager appUpdateManager;

    [Header("Custom Update UI")]
    [SerializeField] private GameObject updatePopup;
    [SerializeField]
    private m_Addressable_Script addressable_Script;

    [SerializeField]
    string URL = "https://play.google.com/store/apps/details?id=com.mailtoalbatrossgamingstudios.PawSomeAdventure&hl=en_IN";
    private void Start()
    {
        Debug.Log("Update Manager");
        // Hide popup initially
        updatePopup.SetActive(false);

        // Start update check
        StartCoroutine(CheckForUpdate());


    }

    IEnumerator CheckForUpdate()
    {
        Debug.Log("try to call");
        appUpdateManager = new AppUpdateManager();

        // Request update info
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;
        Debug.Log("after Return");

        // Check if request successful
        if (appUpdateInfoOperation.IsSuccessful)
        {
            AppUpdateInfo appUpdateInfo = appUpdateInfoOperation.GetResult();

            // Is update available?
            if (appUpdateInfo.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                Debug.Log("Update Available");

                // SHOW YOUR CUSTOM POPUP HERE
                updatePopup.SetActive(true);
            }
            else
            {
                StartCoroutine(StartAddressableAfterDelay());
                Debug.Log("No Update Available");
            }
        }
        else
        {
            StartCoroutine(StartAddressableAfterDelay());
            Debug.Log("Update Check Failed");
        }
    }

    IEnumerator StartAddressableAfterDelay()
    {
        Debug.Log("BeginLoading Called");
        yield return new WaitForSeconds(0f);

        addressable_Script.BeginLoading();
    }


    // Button Function
    public void OnClickUpdate()
    {
        //StartCoroutine(StartFlexibleUpdate());
        Application.OpenURL(URL);
    }

    IEnumerator StartFlexibleUpdate()
    {
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoOperation =
            appUpdateManager.GetAppUpdateInfo();

        yield return appUpdateInfoOperation;

        if (appUpdateInfoOperation.IsSuccessful)
        {
            AppUpdateInfo appUpdateInfo = appUpdateInfoOperation.GetResult();

            // Start Flexible Update
            var startUpdateRequest = appUpdateManager.StartUpdate(
                appUpdateInfo,
                AppUpdateOptions.FlexibleAppUpdateOptions()
            );

            yield return startUpdateRequest;


        }
    }
}
