using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Pyxie
{
    public class PlayerSettings : INotifyPropertyChanged
    {
        private bool useJaZero_;
        private bool useDetection_;
        private bool useExclusions_;
        private bool useSpeed_;
        private float detectedSpeed_;
        private float speed_;
        private bool useGm_;
        private bool useMaintenance_;
        private bool useEngagedMode_;
        private bool useClientBlock_;
        private bool dangerMode_;

        public PlayerSettings()
        {
            Speed = 5.0f;
            DetectedSpeed = 5.0f;
        }

        public bool UseJaZero {
            get { return useJaZero_; }
            set { useJaZero_ = value; RaisePropertyChanged(); } 
        }

        public bool UseDetection {
            get { return useDetection_; }
            set { useDetection_ = value; RaisePropertyChanged(); } 
        }

        public bool UseExclusions {
            get { return useExclusions_; }
            set { useExclusions_ = value; RaisePropertyChanged(); } 
        }

        public bool UseSpeed {
            get { return useSpeed_; }
            set { useSpeed_ = value; RaisePropertyChanged(); } 
        }

        public float DetectedSpeed {
            get { return detectedSpeed_; }
            set { detectedSpeed_ = value >= 5.0f && value <= 32.0f ? value : 5.0f; RaisePropertyChanged(); } 
        }

        public float Speed {
            get { return speed_; }
            set { speed_ = value >= 5.0f && value <= 32.0f ? value : 5.0f; RaisePropertyChanged(); } 
        }

        public bool UseGM
        {
            get { return useGm_; }
            set { useGm_ = value; RaisePropertyChanged(); }
        }

        public bool UseMaintenance
        {
            get { return useMaintenance_; }
            set { useMaintenance_ = value; RaisePropertyChanged(); }
        }

        public bool UseEngagedMode
        {
            get { return useEngagedMode_; }
            set { useEngagedMode_ = value; RaisePropertyChanged(); }
        }

        public bool UseClientBlock
        {
            get { return useClientBlock_; }
            set { useClientBlock_ = value; RaisePropertyChanged(); }
        }

        public bool DangerMode
        {
            get { return dangerMode_; }
            set { dangerMode_ = value; RaisePropertyChanged(); }
        }



        #region "Implement INotifyPropertyChanged"
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged([CallerMemberName] string caller = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(caller));
            }
        }
        #endregion



    }
}
