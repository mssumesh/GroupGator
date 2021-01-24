using System;
using System.Collections.Generic;
using System.Text;

namespace Gator
    {
    public class Constants
        {
        public enum GatherSpeed
            {
            Ultra,
            Quick,
            Medium
            };
        public enum LimitOver
            {
            GatherLimitOver,
            InviteLimitOver,
            TimeLimitOver,
            NotOver
            };

        public enum GroupInitType
            {
            groupid,
            groupname,
            groupurl,
            link
            };

        public enum PlayerInitType
            {
            playername,
            playerid,
            playerurl,
            link
            };

        public enum PlayerStatus
            {
            Offline,
            Online,
            InGame
            };

        public enum SoftLicense
            {
            Demo,
            Paid,
            NotActivated
            };
        public enum InviteErrors
            {
            Fail,
            Success,
            Declined,
            AlreadyInvited,
            NoPermission,
            Error
            };
        public enum GatherType
            {
            All,
            Offline,
            Online,
            InGame            
            };
        public enum InviteDelay
            {
            RandomFast,
            RandomMedium,
            RandomSlow,
            Fast,
            Medium,
            Slow,
            DeadSlow
            };
        public enum FailedDelay
            {
            Instant,
            Random,
            Wait1S,
            Wait2S,
            Wait3S,
            Wait4S,
            Wait5S,
            Wait10S,
            Wait15S,
            Wait20S
            };
        public enum SignInStatus
            {
            LoggedOff,
            Authenticating,
            Transfering,
            LoggedIn,            
            CaptchaReqd,
            SGuardReqd,
            Failed
            };

        public enum AvailableUpdate
            {
            NoUpdate,
            NonCritical,
            Critical
            };
        public enum UpdateMessageType
            {
            Info,
            Question
            };
        public enum UpdateResponse
            {
            No,
            Yes,
            Ok
            };
        public enum UpdateType
            {
            Manual,
            Auto
            };
        public enum DislcaimerResponse
            {
            Agree,
            Decline
            };
        public enum LogMsgType
            {
            Error,
            Success,
            Gator
            };
        public enum ServerResponse
            {
            NoAccount,
            Expired,
            JustLocked,
            NotLocked,
            WrongComputer,
            Valid,
            CaptchaBad,
            CaptchaGood,
            AccountDuped,
            AccountCreated,
            AcheckFailed,
            AcheckSuccess,
            UnlockSuccess,
            UnlockFailNotAdmin,
            UnlockFailWrongAccount,
            Failed,
            Success,
            GiftFailNoDonor,
            GiftFailNoReceiver,
            GiftSuccess,
            GiftFailNotEnough
            };

        public enum FormatTimeType
            {
            Dot,
            Verbose
            };

        public enum DonateType
            {
            Donate1,
            Donate2,
            Donate3,
            Donate4,
            Donate5,
            Donate6
            };

        public enum TabSituation
            {
            DonorLogOff,
            SteamLogOff,
            FreeLogOff,
            DonorLogIn,
            SteamLogIn,
            FreeLogin
            };
        public enum TabPos
            {
            SteamLogin,
            Gather,
            Invite,
            Log,
            Donate,
            Admin
            };

        public struct StructInviteDelay
            {
            public int maxiph ;
            public bool israndom;
            public int maxwait;
            public int curdelay;
            public int faildelay;
            };

        public enum GatherError
            {
            IDAlreadyGathered,
            IDBlackListed,
            Success
            };
        public enum InviteError
            {
            IDAlreadyInvited,
            Failed,
            Success
            };
            
        }
    }
