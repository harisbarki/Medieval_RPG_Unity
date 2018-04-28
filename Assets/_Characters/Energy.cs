using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {

        [SerializeField] Image energyOrb = null;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 10f;

        float currentEnergyPoints = 100f;

        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            UpdateEnergyBar();
        }

        // Update is called once per frame
        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoint();
                UpdateEnergyBar();
            }
        }

        private void AddEnergyPoint()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints);
        }

        public bool IsEnergyAvailable (float amount)
        {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount)
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - amount, 0, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            energyOrb.fillAmount = EnergyAsPercent();
        }

        float EnergyAsPercent()
        {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}
