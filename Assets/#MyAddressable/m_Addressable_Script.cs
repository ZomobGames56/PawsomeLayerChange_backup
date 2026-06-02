using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class m_Addressable_Script : MonoBehaviour
{
    public string sceneKey;

    public TextMeshProUGUI statusText;
    public TextMeshProUGUI percentText;
    public TextMeshProUGUI sizeText;
    public TextMeshProUGUI speedText;

    public Image progressSlider;

    public GameObject noInternetPanel;
    public GameObject retryButton;

    float lastBytes;
    float timer;

    void Start()
    {
        retryButton.SetActive(false);
        noInternetPanel.SetActive(false);

       
    }
    public void BeginLoading()
    {
        retryButton.SetActive(false);
        noInternetPanel.SetActive(false);

        StartCoroutine(StartLoader());
    }
    public void Retry()
    {
        retryButton.SetActive(false);
        noInternetPanel.SetActive(false);
        StartCoroutine(StartLoader());
    }

    IEnumerator StartLoader()
    {
        statusText.text = "Initializing.....";

        var initHandle = Addressables.InitializeAsync();
        yield return initHandle;

        // ❗ Do NOT check Status or Release initHandle

        yield return CheckCatalogUpdate();
        yield return CheckDownload();
    }
    IEnumerator CheckCatalogUpdate()
    {
        //statusText.text = "Checking Updates......";

        var checkHandle = Addressables.CheckForCatalogUpdates();
        yield return checkHandle;

        List<string> catalogs = null;

        try
        {
            if (checkHandle.IsValid() && checkHandle.Status == AsyncOperationStatus.Succeeded)
            {
                catalogs = checkHandle.Result;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Catalog check error: " + e.Message);
        }

        if (checkHandle.IsValid())
        {
            Addressables.Release(checkHandle);
        }

        if (catalogs != null && catalogs.Count > 0)
        {
            //Downloading Essential Asset...(27/100)%
            statusText.text = "Updating Content";

            var updateHandle = Addressables.UpdateCatalogs(catalogs);
            yield return updateHandle;

            try
            {
                if (updateHandle.IsValid() && updateHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    Debug.LogWarning("Catalog update failed");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Update error: " + e.Message);
            }

            if (updateHandle.IsValid())
            {
                Addressables.Release(updateHandle);
            }
        }
    }

    IEnumerator CheckDownload()
    {
        statusText.text = "Checking Files....";

        var sizeHandle = Addressables.GetDownloadSizeAsync(sceneKey);
        yield return sizeHandle;

        if (sizeHandle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Failed to get download size");
            Addressables.Release(sizeHandle);
            yield break;
        }

        long size = sizeHandle.Result;
        Addressables.Release(sizeHandle);

        if (size == 0)
        {
            yield return LoadScene();
        }
        else
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                ShowInternetError();
                yield break;
            }

            yield return Download(size);
            yield return LoadScene();
        }
    }

    IEnumerator Download(long totalBytes)
    {
        //Downloading Essential Asset...(27/100)%
        //statusText.text = "Downloading......";
        //statusText.text  = 

        var handle = Addressables.DownloadDependenciesAsync(sceneKey);

        while (!handle.IsDone)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Addressables.Release(handle);
                ShowInternetError();
                yield break;
            }

            var status = handle.GetDownloadStatus();

            float percent = status.Percent;

            progressSlider.fillAmount = percent;
            percentText.text = (percent * 100f).ToString("F0") + "%";
            //Downloading Essential Asset...(27/100)%

            statusText.text = $"Downloading Essential Assets...({percent * 100f:F0}/100)%";
            float downloadedMB = status.DownloadedBytes / 1024f / 1024f;
            float totalMB = totalBytes / 1024f / 1024f;

            sizeText.text = downloadedMB.ToString("F1") + "MB / " + totalMB.ToString("F1") + "MB";

            CalculateSpeed(status.DownloadedBytes);

            yield return null;
        }

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Download failed");
            Addressables.Release(handle);
            yield break;
        }

        Addressables.Release(handle);
    }

    void CalculateSpeed(long bytes)
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            float speed = (bytes - lastBytes) / 1024f / 1024f;

            speedText.text = speed.ToString("F2") + " MB/s";

            lastBytes = bytes;
            timer = 0;
        }
    }

    IEnumerator LoadScene()
    {
        statusText.text = "Loading Scene....";

        var handle = Addressables.LoadSceneAsync(sceneKey,LoadSceneMode.Single);

        while (!handle.IsDone)
        {
            float percent = handle.PercentComplete;

            progressSlider.fillAmount = percent;
            percentText.text = "("+(percent * 100f).ToString("F0")+"/" +"100)"+ "%";
            statusText.text = $"Loading Scene...({percent * 100f:F0}/100)%";
            yield return null;
        }

        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Scene load failed");
        }
    }

    void ShowInternetError()
    {
        noInternetPanel.SetActive(true);
        retryButton.SetActive(true);
    }
}