﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Interfaces
{
    public interface IAutarizationManager
    {
        void StartManager();
        PlayerData GetPlayerData();
        void SetPlayerData(PlayerData playerData);
    }
}
