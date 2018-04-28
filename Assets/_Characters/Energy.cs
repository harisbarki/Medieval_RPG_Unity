using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {

        [SerializeField] RawImage energyBar = null;
        [SerializeField] float maxEnergyPoints = 100f;

        float currentEnergyPoints = 100f;

        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        // Update is called once per frame
        void Update()
        {

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
            // TODO remove magic numbers
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercent()
        {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}
