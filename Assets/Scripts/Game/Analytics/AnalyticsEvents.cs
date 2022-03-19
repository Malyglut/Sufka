using System;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace Sufka.Game.Analytics
{
    public static class AnalyticsEvents
    {
        private const string GAME_MODE_UNLOCKED_EVENT = "GAME_MODE_UNLOCKED";
        private const string COLOR_SCHEME_SELECTED_EVENT = "COLOR_SCHEME_SELECTED";
        private const string COLOR_SCHEME_POPUP_PRESENTED_EVENT = "COLOR_SCHEME_POPUP_PRESENTED";
        private const string COLOR_SCHEME_UNLOCKED_EVENT = "COLOR_SCHEME_UNLOCKED";
        private const string HINT_POPUP_PRESENTED_EVENT = "HINT_POPUP_PRESENTED";
        private const string NEW_GAME_EVENT = "NEW_GAME";
        private const string CONTINUE_GAME_EVENT = "CONTINUE_GAME";
        private const string HINT_AD_DECLINED_EVENT = "HINT_AD_DECLINED";
        private const string WORD_GUESSED_EVENT = "WORD_GUESSED";
        private const string POINTS_AD_DECLINED_EVENT = "POINTS_AD_DECLINED";
        private const string POINTS_AD_WATCHED_EVENT = "POINTS_AD_WATCHED";
        private const string HINT_AD_WATCHED_EVENT = "HINT_AD_WATCHED";
        private const string GAME_MODE_POPUP_PRESENTED_EVENT = "GAME_MODE_POPUP_PRESENTED";
        private const string HINT_USED_EVENT = "HINT_USED";
        private const string WORD_NOT_GUESSED_EVENT = "WORD_NOT_GUESSED";
        private const string HINT_FOR_PLAYING_EVENT = "HINT_FOR_PLAYING";
        private const string STATISTICS_CHECKED_EVENT = "STATISTICS_CHECKED";

        private const string OPERATING_SYSTEM_FIELD = "operating_system";
        private const string ANDROID_VALUE = "Android";
        private const string IOS_VALUE = "iOS";

        private const string GAME_MODE_FIELD = "game_mode";
        private const string COLOR_SCHEME_FIELD = "color_scheme";
        private const string DATE_TIME_FIELD = "date_time";
        private const string ATTEMPT_FIELD = "attempt";
        private const string TARGET_WORD_FIELD = "target_word";
        private const string POINTS_FOR_AD_FIELD = "points_for_ad";
        private const string CURRENT_POINTS_FIELD = "current_points";
        private const string AVAILABLE_HINTS_FIELD = "available_hints";

        private static void SendEvent(string eventName, params (string, object)[] fields)
        {
            var eventData = new Dictionary<string, object>
                            {
#if UNITY_ANDROID
                                {OPERATING_SYSTEM_FIELD, ANDROID_VALUE},
#elif UNITY_IOS
                                {OPERATING_SYSTEM_FIELD, IOS_VALUE},
#endif
                                {DATE_TIME_FIELD, DateTime.Now}
                            };

            foreach (var (field, value) in fields)
            {
                eventData.Add(field, value);
            }

            AnalyticsEvent.Custom(eventName, eventData);
        }

        public static void GameModeUnlocked(string gameModeName)
        {
            SendEvent(GAME_MODE_UNLOCKED_EVENT, (GAME_MODE_FIELD, gameModeName));
        }

        public static void ColorSchemeSelected(string colorSchemeName)
        {
            SendEvent(COLOR_SCHEME_SELECTED_EVENT, (COLOR_SCHEME_FIELD, colorSchemeName));
        }

        public static void ColorSchemeUnlockPopup(string colorSchemeName, int currentPoints)
        {
            SendEvent(COLOR_SCHEME_POPUP_PRESENTED_EVENT,
                      (COLOR_SCHEME_FIELD, colorSchemeName),
                      (CURRENT_POINTS_FIELD, currentPoints));
        }

        public static void ColorSchemeUnlocked(string colorSchemeName)
        {
            SendEvent(COLOR_SCHEME_UNLOCKED_EVENT, (COLOR_SCHEME_FIELD, colorSchemeName));
        }

        public static void HintPopup()
        {
            SendEvent(HINT_POPUP_PRESENTED_EVENT);
        }

        public static void NewGame(string gameModeName)
        {
            SendEvent(NEW_GAME_EVENT, (GAME_MODE_FIELD, gameModeName));
        }

        public static void ContinueGame(string gameModeName)
        {
            SendEvent(CONTINUE_GAME_EVENT, (GAME_MODE_FIELD, gameModeName));
        }

        public static void HintAdDeclined()
        {
            SendEvent(HINT_AD_DECLINED_EVENT);
        }

        public static void WordGuessed(string gameModeName, int attempt, string targetWord)
        {
            SendEvent(WORD_GUESSED_EVENT,
                      (GAME_MODE_FIELD, gameModeName),
                      (ATTEMPT_FIELD, attempt),
                      (TARGET_WORD_FIELD, targetWord));
        }

        public static void PointsAdDeclined(int pointsForAd)
        {
            SendEvent(POINTS_AD_DECLINED_EVENT, (POINTS_FOR_AD_FIELD, pointsForAd));
        }

        public static void PointsAdWatched(int pointsForAd)
        {
            SendEvent(POINTS_AD_WATCHED_EVENT, (POINTS_FOR_AD_FIELD, pointsForAd));
        }

        public static void HintAdWatched()
        {
            SendEvent(HINT_AD_WATCHED_EVENT);
        }

        public static void GameModePopup(string gameModeName, int currentPoints)
        {
            SendEvent(GAME_MODE_POPUP_PRESENTED_EVENT,
                      (GAME_MODE_FIELD, gameModeName),
                      (CURRENT_POINTS_FIELD, currentPoints));
        }

        public static void HintUsed(string gameModeName, string targetWord, int attempt, int availableHints)
        {
            SendEvent(HINT_USED_EVENT,
                      (GAME_MODE_FIELD, gameModeName),
                      (TARGET_WORD_FIELD, targetWord),
                      (ATTEMPT_FIELD, attempt),
                      (AVAILABLE_HINTS_FIELD, availableHints));
        }

        public static void WordNotGuessed(string gameModeName, string targetWord)
        {
            SendEvent(WORD_NOT_GUESSED_EVENT,
                      (GAME_MODE_FIELD, gameModeName),
                      (TARGET_WORD_FIELD, targetWord));
        }

        public static void HintForPlaying(int availableHints)
        {
            SendEvent(HINT_FOR_PLAYING_EVENT, (AVAILABLE_HINTS_FIELD, availableHints));
        }

        public static void StatisticsChecked(string gameModeName)
        {
            SendEvent(STATISTICS_CHECKED_EVENT, (GAME_MODE_FIELD, gameModeName));
        }
    }
}
