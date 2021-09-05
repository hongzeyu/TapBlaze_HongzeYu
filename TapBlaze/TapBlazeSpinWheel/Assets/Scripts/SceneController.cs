using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public GameObject wheel;
    public GameObject result;
    public Sprite brush, coin, gem, hammer, heart;
    private float timeInterval;
    //1-8
    private int rewardresult;
    int[] resulttable;
    string rewardtext;
    private Vector3 scaleChange1 = new Vector3(0.008f, 0.008f, 0.008f);
    private Vector3 scaleChange2 = new Vector3(0.006f, 0.006f, 0.006f);
    private Vector3 positionChange1 = new Vector3(0.0f, -0.025f, 0.0f);
    private Vector3 positionChange2 = new Vector3(0.0f, -0.0355f, 0.0f);
    /*
     * Cycle:
     * 1: Life 30 0.2
     * 2. Brush 3 0.1
     * 3. Gem 35 0.1
     * 4. Hammer 3 0.1
     * 5. Coin 750 0.05
     * 6. Brush 1 0.2
     * 7. Gem 75 0.05
     * 8. Hammer 1 0.2
     */

    //Unit test for result probabilities of outcome 1-8
    public void UnitTest()
    {
        resulttable = new int[8];
        for (int i = 0; i < 1000; ++i)
        {
            SpinForTest();
            resulttable[rewardresult - 1] += 1;

        }
        for (int j = 0; j < 8; ++j)
        {
            Debug.Log("Result " + (j + 1).ToString() + ": " + resulttable[j].ToString());
        }
    }
    public void Spinwheel()
    {
        StartCoroutine(Spin());

        
    }
    private void SpinForTest()
    {
        //choose outcome based on probabilities
        int getresult = Random.Range(1, 101);
        if (getresult <= 20)
        {
            rewardresult = 1;
        }
        else if (getresult <= 30)
        {
            rewardresult = 2;
        }
        else if (getresult <= 40)
        {
            rewardresult = 3;
        }
        else if (getresult <= 50)
        {
            rewardresult = 4;
        }
        else if (getresult <= 55)
        {
            rewardresult = 5;
        }
        else if (getresult <= 75)
        {
            rewardresult = 6;
        }
        else if (getresult <= 80)
        {
            rewardresult = 7;
        }
        else
        {
            rewardresult = 8;
        }
    }
    private IEnumerator Spin()
    {
        GameObject button = GameObject.FindGameObjectWithTag("SpinButton");
        if (button.GetComponentInChildren<Text>().text != "Claim")
        {

            GameObject.FindGameObjectWithTag("SpinButton").SetActive(false);
            wheel = GameObject.FindGameObjectWithTag("Wheel");
            //Default Spin 2 cycles
            for (int i = 0; i < 240; ++i)
            {
                wheel.transform.Rotate(0, 0, 3.0f);
                yield return new WaitForSeconds(0.01f);
            }
            //Slow Spin 1-2 cycles
            int slowspin = Random.Range(1, 3);
            slowspin *= 180;
            for (int j = 0; j < slowspin; ++j)
            {
                wheel.transform.Rotate(0, 0, 2.0f);
                yield return new WaitForSeconds(0.01f);
            }
            //get final spin
            //if reward = x, spin angle is 45*(x-1)+22.5=45x-22.5=0.5*(90x-45) = 4.5*(10x-5) = 1.5*(30x-15)
            int getresult = Random.Range(1, 101);
            int finalspin;
            if (getresult <= 20)
            {
                rewardresult = 1;
                //set the rewards animation
                rewardtext = "30min";
                result.GetComponent<SpriteRenderer>().sprite = heart;
            }
            else if (getresult <= 30)
            {
                rewardresult = 2;
                rewardtext = "x3";
                result.GetComponent<SpriteRenderer>().sprite = brush;
            }
            else if (getresult <= 40)
            {
                rewardresult = 3;
                rewardtext = "x35";
                result.GetComponent<SpriteRenderer>().sprite = gem;
            }
            else if (getresult <= 50)
            {
                rewardresult = 4;
                rewardtext = "x3";
                result.GetComponent<SpriteRenderer>().sprite = hammer;
            }
            else if (getresult <= 55)
            {
                rewardresult = 5;
                rewardtext = "x750";
                result.GetComponent<SpriteRenderer>().sprite = coin;
            }
            else if (getresult <= 75)
            {
                rewardresult = 6;
                rewardtext = "x1";
                result.GetComponent<SpriteRenderer>().sprite = brush;
            }
            else if (getresult <= 80)
            {
                rewardresult = 7;
                rewardtext = "x75";
                result.GetComponent<SpriteRenderer>().sprite = gem;
            }
            else
            {
                rewardresult = 8;
                rewardtext = "x1";
                result.GetComponent<SpriteRenderer>().sprite = hammer;
            }
            finalspin = rewardresult * 30;
            finalspin -= 15;
            //split spin time in half to gradually reduce spin speed and have smoother transition
            int slowest = finalspin / 4;
            finalspin -= slowest;
            slowest *= 3;
            for (int k = 0; k < finalspin; ++k)
            {
                wheel.transform.Rotate(0, 0, 1.5f);
                yield return new WaitForSeconds(0.01f);
            }
            for (int m = 0; m < slowest; ++m)
            {
                wheel.transform.Rotate(0, 0, 0.5f);
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(2.0f);
            wheel.SetActive(false);
            GameObject.FindGameObjectWithTag("RewardText").SetActive(true);
            GameObject.FindGameObjectWithTag("Border").SetActive(false);
            GameObject.FindGameObjectWithTag("Arrow").SetActive(false);
            button.SetActive(true);
            result.SetActive(true);
            GameObject.FindGameObjectWithTag("SpinButton").GetComponentInChildren<Text>().text = "Claim";
            GameObject.FindGameObjectWithTag("RewardText").GetComponentInChildren<Text>().text = rewardtext;
            //reward animation
            for (int n = 0; n < 100; ++n)
            {
                result.transform.localScale += scaleChange1;
                result.transform.position += positionChange1;
                GameObject.FindGameObjectWithTag("RewardText").transform.localScale += scaleChange2;
                GameObject.FindGameObjectWithTag("RewardText").transform.position += positionChange2;
                yield return new WaitForSeconds(0.01f);
            }
        }

    }
}
