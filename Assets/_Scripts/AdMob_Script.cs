using UnityEngine;
using System;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Api;

public class AdMob_Script : MonoBehaviour 
{

	private const string BANNER_UNIT_ID_ANDROID = "ca-app-pub-9864397419142722/9288920499";
	private const string BANNER_UNIT_ID_IOS = "ca-app-pub-9864397419142722/2799904896";

    private const string INTERSTITIAL_UNIT_ID_ANDROID = "ca-app-pub-9864397419142722/8706837692";
    private const string INTERSTITIAL_UNIT_ID_IOS = "ca-app-pub-9864397419142722/7230104494";

    private BannerView bannerView;
    private InterstitialAd interstitial;

    private int adCount = 0;

    private int interstitialFreq = 10;

    void Start(){
        adCount = 0;
        Debug.Log("START ADMOB");
    	RequestBanner();
        RequestInterstitial();
        HideBanner();
    }

    public void ShowAd(){
        adCount++;
        Debug.Log("ADMOB - count:" + adCount);

        if( (adCount%interstitialFreq) == 0){
            if (interstitial.IsLoaded()) {
                Debug.Log("ADMOB - interstitial");
              interstitial.Show();
            }
        }

        else{
            ShowBanner();
        }

    }

    public void HideAd(){
        if( (adCount%interstitialFreq) == 0){
            RequestInterstitial();
        }
        else{
            HideBanner();
        }

    }

    public void ShowBanner(){
        Debug.Log("ADMOB - show");
    	bannerView.Show();
    }

    public void HideBanner(){
        Debug.Log("ADMOB - hide");
    	bannerView.Hide();
    }


    private void RequestBanner()
    {
        Debug.Log("ADMOB - request");
        #if UNITY_EDITOR
            string adUnitId = "unused";
        #elif UNITY_ANDROID
            string adUnitId = BANNER_UNIT_ID_ANDROID;
            Debug.Log("ADMOB - android");
        #elif UNITY_IPHONE
            string adUnitId = BANNER_UNIT_ID_IOS;
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create a 320x50 banner at the top of the screen.
       // bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);
        bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        // Register for ad events.
        bannerView.AdLoaded += HandleAdLoaded;
        bannerView.AdFailedToLoad += HandleAdFailedToLoad;
        bannerView.AdOpened += HandleAdOpened;
        bannerView.AdClosing += HandleAdClosing;
        bannerView.AdClosed += HandleAdClosed;
        bannerView.AdLeftApplication += HandleAdLeftApplication;
        // Load a banner ad.
        bannerView.LoadAd(createAdRequest());
    }

    private void RequestInterstitial()
    {
        #if UNITY_EDITOR
            string adUnitId = "unused";
        #elif UNITY_ANDROID
            string adUnitId = INTERSTITIAL_UNIT_ID_ANDROID;
        #elif UNITY_IPHONE
            string adUnitId = INTERSTITIAL_UNIT_ID_IOS;
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create an interstitial.
        interstitial = new InterstitialAd(adUnitId);
        // Register for ad events.
        interstitial.AdLoaded += HandleInterstitialLoaded;
        interstitial.AdFailedToLoad += HandleInterstitialFailedToLoad;
        interstitial.AdOpened += HandleInterstitialOpened;
        interstitial.AdClosing += HandleInterstitialClosing;
        interstitial.AdClosed += HandleInterstitialClosed;
        interstitial.AdLeftApplication += HandleInterstitialLeftApplication;
        // Load an interstitial ad.
        interstitial.LoadAd(createAdRequest());
    }

    // Returns an ad request with custom ad targeting.
    private AdRequest createAdRequest()
    {
        Debug.Log("ADMOB - create request");

       /* return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("6A47814CF16EBC5493285D4C591C5DC2")
                .AddKeyword("game")
                .SetGender(Gender.Male)
                .SetBirthday(new DateTime(1985, 1, 1))
                .TagForChildDirectedTreatment(false)
                .AddExtra("color_bg", "9B30FF")
                .Build();
        */

        /* return new AdRequest.Builder()
                .AddTestDevice(AdRequest.TestDeviceSimulator)
                .AddTestDevice("6A47814CF16EBC5493285D4C591C5DC2")
                .Build();
        */

                return new AdRequest.Builder()
                .AddKeyword("game")
                .Build();

    }

    private void ShowInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        else
        {
            print("Interstitial is not ready yet.");
        }
    }

    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received.");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    void HandleAdClosing(object sender, EventArgs args)
    {
        print("HandleAdClosing event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstitial callback handlers

    public void HandleInterstitialLoaded(object sender, EventArgs args)
    {
        print("HandleInterstitialLoaded event received.");
    }

    public void HandleInterstitialFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleInterstitialFailedToLoad event received with message: " + args.Message);
    }

    public void HandleInterstitialOpened(object sender, EventArgs args)
    {
        print("HandleInterstitialOpened event received");
    }

    void HandleInterstitialClosing(object sender, EventArgs args)
    {
        print("HandleInterstitialClosing event received");
    }

    public void HandleInterstitialClosed(object sender, EventArgs args)
    {
        print("HandleInterstitialClosed event received");
    }

    public void HandleInterstitialLeftApplication(object sender, EventArgs args)
    {
        print("HandleInterstitialLeftApplication event received");
    }

    #endregion
}