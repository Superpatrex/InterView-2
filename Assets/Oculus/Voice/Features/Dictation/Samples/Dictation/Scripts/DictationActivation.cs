/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Meta.WitAi.Dictation;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Meta.Voice.Samples.Dictation
{
    public class DictationActivation : MonoBehaviour
    {
        [FormerlySerializedAs("dictation")]
        [SerializeField] private TMP_Text button;
        [SerializeField] private DictationService _dictation;
        

        private bool first = true;
        private bool messageReady = false;
        private bool isActive;
        public void ToggleActivation()
        {
            //if (first)
            //{
            //    first = false;
            //    return;
            //}
            print("Toggle Activation");
            if (_dictation.MicActive )
            {
                _dictation.Deactivate();
                _dictation.Cancel();
                button.text = "Record";
                messageReady = true;
            }
            else
            {
                _dictation.Activate();
                button.text = "Stop Recording";
            }
        }

        public bool ReadyToSend()
        {
            return messageReady;
        }

        public void ResetReady()
        {
            messageReady = false;
        }
    }
}
