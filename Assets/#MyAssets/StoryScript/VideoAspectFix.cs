using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(RawImage))]
public class VideoAspectFix : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private AspectRatioFitter fitter;

    void Start()
    {
        fitter = GetComponent<AspectRatioFitter>();

        if (videoPlayer.texture != null)
        {
            float ratio = (float)videoPlayer.texture.width / videoPlayer.texture.height;
            fitter.aspectRatio = ratio;
        }
    }
}