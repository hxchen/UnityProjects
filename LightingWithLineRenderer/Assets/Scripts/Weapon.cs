using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private List<Creature> allCreatures;

    private List<LightingController> allLines;

    [SerializeField]
    private LightingController linePrefab;

    private bool weaponIsOn;

    [SerializeField]
    private Transform origin;

    private void Awake() {
        allLines = new List<LightingController>();
        for (int i = 0; i < allCreatures.Count; i++) {
            LightingController line = Instantiate(linePrefab);
            allLines.Add(line);
            line.AssignTarget(origin.position, allCreatures[i].transform);
            line.gameObject.SetActive(false);
        }
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ToggleWeapon();   
        }
    }
    private void ToggleWeapon() {
        if (weaponIsOn) {
            foreach (var line in allLines) {
                line.gameObject.SetActive(false);
            }
            foreach (var creature in allCreatures) {
                creature.StopLightingShock();
            }
            weaponIsOn = false;
        } else {
            foreach (var line in allLines) {
                line.gameObject.SetActive(true);
            }
            foreach (var creature in allCreatures) {
                creature.StartLightingShock();
            }
            weaponIsOn = true;
        }
    }
}
