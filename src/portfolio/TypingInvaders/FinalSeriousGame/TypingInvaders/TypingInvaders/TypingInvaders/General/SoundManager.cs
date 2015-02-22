using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TypingInvaders
{
    public enum SoundType
    {
        Music,
        Effect,
    }

    public class SoundName
    {
        public const string Alarm = "alarm_2";
        public const string Clap = "claps";
        public const string Computer = "computer";
        public const string Explosion = "explosion_short";
        public const string HitElectronic = "hit_electronic";
        public const string Missile = "missile";
        public const string SynthBeep = "synth_beep_3";
        public const string TranceHouseLoop = "tranceHouseLoop";
    }

    class SoundManager
    {
        public AudioEngine engine;
        public SoundBank soundBank;
        public WaveBank waveBank;

        AudioCategory effectCategory;
        AudioCategory musicCategory;

        public SoundManager()
        {
            engine = new AudioEngine("Content/Audio/SoundForTypingInvaders.xgs");
            soundBank = new SoundBank(engine, "Content/Audio/Sound Bank.xsb");
            waveBank = new WaveBank(engine, "Content/Audio/Wave Bank.xwb");

            musicCategory = engine.GetCategory("Music");
            effectCategory = engine.GetCategory("Effect");
        }

        public void Update()
        {
            engine.Update();
        }

        public void PlayCue(SoundType soundType, string soundName)
        {
            soundBank.PlayCue(soundName);
        }

        public void MusicVolume(float amountToChangeBy)
        {
            musicCategory.SetVolume(amountToChangeBy);
        }

        public void EffectVolume(float amountToChangeBy)
        {
            effectCategory.SetVolume(amountToChangeBy);
        }
    }
}
