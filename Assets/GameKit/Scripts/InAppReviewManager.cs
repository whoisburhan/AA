//#if UNITY_ANDROID || UNITY_IOS
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Google.Play.Review;

//public class InAppReviewManager : MonoBehaviour
//{
//    public static InAppReviewManager Instance { get; private set; }

//    private ReviewManager _reviewManager;
//    private PlayReviewInfo _playReviewInfo;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//        DontDestroyOnLoad(this);
//    }

//    public IEnumerator RequestReview()
//    {
//        _reviewManager = new ReviewManager();

//        // Request a review object
//        var requestFlowOperation = _reviewManager.RequestReviewFlow();
//        yield return requestFlowOperation;
//        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
//        {
//            // Log error. For example, using requestFlowOperation.Error.ToString().
//            yield break;
//        }
//        _playReviewInfo = requestFlowOperation.GetResult();

//        // Launch the In App Review flow
//        var launchFlowOperation = _reviewManager.LaunchReviewFlow(_playReviewInfo);
//        yield return launchFlowOperation;
//        _playReviewInfo = null; // Reset the object
//        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
//        {
//            // Log error. For example, using requestFlowOperation.Error.ToString().
//            yield break;
//        }
//    }
//}
//#endif