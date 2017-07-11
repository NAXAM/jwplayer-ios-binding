using System;
using System.Linq;
using CoreGraphics;
using Foundation;
using JWPlayer;
using UIKit;

namespace JWPlayerQs
{
    public partial class ViewController : UIViewController, IJWPlayerDelegate
    {
        JWPlayerController player;

        protected ViewController(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            CreatePlayer();
            SetupNotifications();
            View.AddSubview(player.View);
        }

        void CreatePlayer()
        {
            //MARK: JWConfig

            /* JWConfig can be created with a single file reference */
            //     JWConfig *config = [JWConfig configWithContentURL:@"http://content.bitsontherun.com/videos/3XnJSIm4-injeKYZS.mp4"];

            JWConfig config = new JWConfig();
            config.Sources = new JWSource[] {
                new JWSource(@"http://content.bitsontherun.com/videos/bkaovAYt-injeKYZS.mp4", @"180p Streaming", true),
                JWSource.SourceWithFile(@"http://content.bitsontherun.com/videos/bkaovAYt-52qL9xLP.mp4",@"270p Streaming"),
                JWSource.SourceWithFile(@"http://content.bitsontherun.com/videos/bkaovAYt-DZ7jSYgM.mp4",@"720p Streaming")
            };

            config.Image = @"http://content.bitsontherun.com/thumbs/bkaovAYt-480.jpg";
            config.Title = @"JWPlayer Demo";
            config.Controls = true;  //default
            config.Repeat = false;   //default
            config.PremiumSkin = JWPremiumSkin.Roundster;

            /* custom css skin can be applied using */
            //     config.cssSkin = @"http:p.jwpcdn.com/iOS/Skins/ethan.css";

            //MARK: JWTrack (captions)
            config.Tracks = new JWTrack[]{
                new JWTrack(@"http://playertest.longtailvideo.com/caption-files/sintel-en.srt", @"English", true),
                JWTrack.TrackWithFile(@"http://playertest.longtailvideo.com/caption-files/sintel-sp.srt",@"Spanish"),
                JWTrack.TrackWithFile(@"http://playertest.longtailvideo.com/caption-files/sintel-ru.srt",@"Russian")
            };


            //MARK: JWCaptionStyling
            JWCaptionStyling captionStyling = new JWCaptionStyling();
            captionStyling.Font = UIFont.FromName(@"Zapfino", 20);
            captionStyling.EdgeStyle = JWEdgeStyle.raised;
            captionStyling.WindowColor = UIColor.Orange;
            captionStyling.BackgroundColor = UIColor.FromRGBA(0.3f, 0.6f, 0.3f, 0.7f);
            captionStyling.FontColor = UIColor.Blue;
            config.CaptionStyling = captionStyling;

            //MARK: JWAdConfig
            JWAdConfig adConfig = new JWAdConfig();
            adConfig.AdMessage = @"Ad duration countdown xx";
            adConfig.SkipMessage = @"Skip in xx";
            adConfig.SkipText = @"Move on";
            adConfig.SkipOffset = 3;
            adConfig.AdClient = JWAdClient.vastPlugin;
            config.AdConfig = adConfig;

            //    config.autostart = YES;

            //MARK: Waterfall Tags
            var waterfallTags = new[] { @"bad tag", @"another bad tag", @"http://playertest.longtailvideo.com/adtags/preroll_newer.xml" };

            //MARK: JWAdBreak
            config.AdSchedule = new[] {
                JWAdBreak.AdBreakWithTags(waterfallTags.Select(x => new NSString(x)).Cast<NSObject>().ToArray(), @"pre"),
                JWAdBreak.AdBreakWithTag(@"http://playertest.longtailvideo.com/adtags/preroll_newer.xml",@"0:00:05"),
                JWAdBreak.AdBreakWithTag(@"http://demo.jwplayer.com/player-demos/assets/overlay.xml",@"7", true),
                //                        JWAdBreak.AdBreakWithTag(@"http://playertest.longtailvideo.com/adtags/preroll_newer.xml",@"5"),
                JWAdBreak.AdBreakWithTag(@"http://playertest.longtailvideo.com/adtags/preroll_newer.xml",@"50%"),
                JWAdBreak.AdBreakWithTag(@"http://playertest.longtailvideo.com/adtags/preroll_newer.xml",@"post")
            };

            player = new JWPlayerController(config)
            {
                WeakDelegate = this
            };

            var frame = new CGRect(View.Bounds.X, 64, View.Bounds.Width, View.Bounds.Height / 2 - 44 - 64);

            player.View.Frame = frame;
            player.View.AutoresizingMask = UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleWidth;

            player.OpenSafariOnAdClick = true;
            player.ForceFullScreenOnLandscape = true;
            player.ForceLandscapeOnFullScreen = true;

            playButton.TouchUpInside += delegate
            {
                if (player.PlayerState == "PAUSED" || player.PlayerState == "IDLE")
                {
                    player.Play();
                    playButton.SetTitle("Pause", UIControlState.Normal);
                }
                else
                {
                    player.Pause();
                    playButton.SetTitle("Play", UIControlState.Normal);
                }
            };

            insertAdButton.TouchUpInside += InsertAd;
        }


        void InsertAd(object sender, EventArgs e)
        {
            player.PlayAd("http://playertest.longtailvideo.com/adtags/preroll_newer.xml");
        }


        void SetupNotifications()
        {
            var notifications = new[] {
               JWPlayerConstants.JWPlayerStateChangedNotification,
               JWPlayerConstants.JWMetaDataAvailableNotification,
               JWPlayerConstants.JWAdActivityNotification,
               JWPlayerConstants.JWErrorNotification,
               JWPlayerConstants.JWCaptionsNotification,
               JWPlayerConstants.JWVideoQualityNotification,
               JWPlayerConstants.JWPlaybackPositionChangedNotification,
               JWPlayerConstants.JWFullScreenStateChangedNotification,
               JWPlayerConstants.JWAdClickNotification
            };

            var center = NSNotificationCenter.DefaultCenter;

            for (int i = 0; i < notifications.Length; i++)
            {
                center.AddObserver(new NSString(notifications[i]), HandleNotification, null);
            }

            center.AddObserver(new NSString(JWPlayerConstants.JWPlaybackPositionChangedNotification), UpdatePlaybackTimer, null);
            center.AddObserver(new NSString(JWPlayerConstants.JWPlayerStateChangedNotification), PlayerStateChanged, null);
            center.AddObserver(new NSString(JWPlayerConstants.JWAdActivityNotification), PlayerStateChanged, null);
        }

        void HandleNotification(NSNotification notificaiton)
        {
            NSDictionary userInfo = notificaiton.UserInfo;
            var callback = userInfo.ObjectForKey(new NSString(@"event")).ToString();

            if (callback.Equals(@"onTime")) { return; }

            var text = callbacksView.Text + $@"\n{callback} {userInfo.Description}";
            callbacksView.Text = text;

            var size = callbacksView.SizeThatFits(new CGSize(callbacksView.Frame.Size.Width, nfloat.MaxValue));

            callbacksView.ContentSize = new CGSize(callbacksView.ContentSize.Width, size.Height);

            if (callbacksView.ContentSize.Height > callbacksView.Frame.Size.Height)
            {
                callbacksView.SetContentOffset(new CGPoint(0, callbacksView.ContentSize.Height - callbacksView.Frame.Size.Height), true);
            }
        }

        void UpdatePlaybackTimer(NSNotification notification)
        {
            var userInfo = notification.UserInfo;

            var callback = userInfo.ObjectForKey(new NSString(@"event")).ToString();

            if (callback.Equals(@"onTime"))
            {
                var position = $"{userInfo.ObjectForKey(new NSString("position"))}/{userInfo.ObjectForKey(new NSString("duration"))}";
                playbackTime.Text = position;
            }
        }


        void PlayerStateChanged(NSNotification info)
        {
            var userInfo = info.UserInfo;
            var @event = userInfo.ObjectForKey(new NSString("event")).ToString();
            if (@event.Equals(@"onPause") ||
               @event.Equals(@"onReady") ||
               @event.Equals(@"onAdPause"))
            {
                playButton.SetTitle("Play", UIControlState.Normal);
            }
            else
            {
                playButton.SetTitle("Pause", UIControlState.Normal);
            }
        }

    }
}
