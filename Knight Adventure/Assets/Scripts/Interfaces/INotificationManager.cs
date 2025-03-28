﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    public interface INotificationManager
    {
        void OpenNotificationWindow(string name);
        void PlayNotificationAudio(string name);
        void StartManager();
        void HandleError(string errorMessage, int numberOfError);

    }
}
