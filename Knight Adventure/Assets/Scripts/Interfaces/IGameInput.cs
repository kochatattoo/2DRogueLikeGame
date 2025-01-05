using System;
using UnityEngine;

public interface IGameInput
{
     // Метод для получения векторного направления движения игрока
      Vector2 GetMovementVector();

      // Метод для получения текущей позиции мыши на экране
      Vector3 GetMousePosition();

      // Метод для преобразования позиции мыши с экрана в мировые координаты
      Vector3 GetMousePositionToScreenWorldPoint();

      // Методы для включения и отключения движения
      void EnableMovement();
      void DisableMovement();

      // События, связанные с игроком
      event EventHandler OnPlayerAttack;
      event EventHandler OnPlayerPause;
      event EventHandler OnPlayerMagicAttack;
      event EventHandler OnPlayerRangeAttack;
      event EventHandler OnPlayerOpen;
}

