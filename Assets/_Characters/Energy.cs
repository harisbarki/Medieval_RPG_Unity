using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {

        [SerializeField] RawImage energyBar;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float pointsPerHit = 10f;

        CameraRaycaster cameraRaycaster = null;
        float currentEnergyPoints = 100f;

        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if(Input.GetMouseButtonDown(1))
            {
                UpdateEnergyPoints();
                UpdateEnergyBar();
            }
        }

        private void UpdateEnergyPoints()
        {
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints - pointsPerHit, 0, maxEnergyPoints);
        }

        private void UpdateEnergyBar()
        {
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercent()
        {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}
