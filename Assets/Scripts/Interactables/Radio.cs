using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SubDrone {
    public class Radio : Interactable {
        public bool playing;

        public AudioClip interaction;
        public AudioClip song;

        private AudioSource m_AudioSource;

        public override void Interact() {
            if (playing) {
                m_AudioSource.Stop();
                m_AudioSource.clip = interaction;
                m_AudioSource.mute = false;
            }

            throw new System.NotImplementedException();
        }
    }
}
