using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Camp : MonoBehaviour {

    public Vector2 position;
    private GameObject capBar;
    public GameObject canvas;
    public Slider capBarSlider;
    public TMP_Text capBarTimerText;
    public float maxCapTime;
    public bool frendly;
    private bool caped;
    private float globalCapedTime;
    private List<CapedEnemy> tankCount = new List<CapedEnemy>();
    public Sprite campFrendly;
    public Sprite campEnemy;

    private class CapedEnemy {
        public float capedTime;
        public GameObject tank;
    }

    void Start() {
        canvas = GameObject.Find("Canvas");
        capBar = GameObject.Find("CapBar");
        capBarSlider = capBar.GetComponent<Slider>();
        capBarTimerText = GameObject.Find("Timer").GetComponent<TextMeshProUGUI>();

        globalCapedTime = 0.0f;
        caped = false;
        maxCapTime = 30.0f;
        capBarSlider.maxValue = maxCapTime;
        capBar.SetActive(false);
    }

    public void setFrendly(bool frend) {
        frendly = frend;

        if (frendly) {
            this.GetComponent<SpriteRenderer>().sprite = campFrendly;
        } else {
            this.GetComponent<SpriteRenderer>().sprite = campEnemy;
        }
    }

    private string CurrentTimer(int enemyCount, float capedTime) {
        return "" + (int)((maxCapTime - capedTime)/enemyCount);
    }

    // Update is called once per frame
    void Update() {
        if (caped) {
            globalCapedTime = 0.0f;

            foreach (CapedEnemy enemy in tankCount) {
                enemy.capedTime += Time.deltaTime;
                globalCapedTime += enemy.capedTime;
            }

            capBarSlider.value = globalCapedTime;
            capBarTimerText.text = CurrentTimer(tankCount.Count, globalCapedTime);

            if (capBarSlider.value >= maxCapTime) {
                caped = false;
                this.GetComponent<CircleCollider2D>().enabled = false;
                this.GetComponent<CircleCollider2D>().enabled = false;
                frendly = !frendly;

                if (frendly) {
                    this.GetComponent<SpriteRenderer>().sprite = campFrendly;
                } else {
                    this.GetComponent<SpriteRenderer>().sprite = campEnemy;
                }

                //Utility.GameOver();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {

        // TODO Set to Enemy
        if (collision.CompareTag("Player")) {

            CapedEnemy deleteTank = new CapedEnemy();
            foreach (CapedEnemy enemy in tankCount) {
                if(GameObject.ReferenceEquals(enemy.tank, collision.gameObject)) {
                    deleteTank = enemy;
                }
            }

            tankCount.Remove(deleteTank);

            if(tankCount.Count <= 0) {
                caped = false;
                capBarSlider.value = 0.0f;
                capBar.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {

        // TODO Set to Enemy
        if (collision.CompareTag("Player") && !this.frendly) {
            CapedEnemy enemy = new CapedEnemy();
            enemy.tank = collision.gameObject;
            enemy.capedTime = 0.0f;

            tankCount.Add(enemy);

            caped = true;
            capBar.SetActive(true);
        }
    }
}
