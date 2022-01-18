using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinManager : MonoBehaviour
{
    [System.Serializable]
    public class WorkerSkin
    {
        public bool unlocked = false;
        public Sprite front;
        public Sprite back;
    }

    public List<WorkerSkin> workerSkins = new List<WorkerSkin>();
    private int selectedSkin = 0;
    [SerializeField]
    private GameObject workerLeft, workerSelected, workerRight;

    public void viewSkins(int direction)
    {
        if (direction > 0) //change skin right
        {
            selectedSkin++;

            //middle skin
            if (selectedSkin == workerSkins.Count)
            {
                selectedSkin = 0;
                workerSelected.GetComponent<Image>().sprite = workerSkins[selectedSkin].front;
            }else
                workerSelected.GetComponent<Image>().sprite = workerSkins[selectedSkin].front;

            //right skin
            if (selectedSkin + 1 == workerSkins.Count)
                workerRight.GetComponent<Image>().sprite = workerSkins[0].front;
            else
                workerRight.GetComponent<Image>().sprite = workerSkins[selectedSkin + 1].front;

            //left skin
            if (selectedSkin - 1 < 0)
                workerLeft.GetComponent<Image>().sprite = workerSkins[workerSkins.Count - 1].front;
            else
                workerLeft.GetComponent<Image>().sprite = workerSkins[selectedSkin - 1].front;
        }
        else if(direction < 0) //change skin left
        {
            selectedSkin--;

            //middle skin
            if (selectedSkin < 0)
            {
                selectedSkin = workerSkins.Count - 1;
                workerSelected.GetComponent<Image>().sprite = workerSkins[selectedSkin].front;
            }
            else
                workerSelected.GetComponent<Image>().sprite = workerSkins[selectedSkin].front;

            //right skin
            if (selectedSkin + 1 == workerSkins.Count)
                workerRight.GetComponent<Image>().sprite = workerSkins[0].front;
            else
                workerRight.GetComponent<Image>().sprite = workerSkins[selectedSkin + 1].front;

            //left skin
            if (selectedSkin - 1 < 0)
                workerLeft.GetComponent<Image>().sprite = workerSkins[workerSkins.Count - 1].front;
            else
                workerLeft.GetComponent<Image>().sprite = workerSkins[selectedSkin - 1].front;

        }else if(direction == 0) //hire button pressed
        {
            workerSelected.GetComponent<Image>().sprite = workerSkins[selectedSkin].front;

            //right skin
            if (selectedSkin + 1 == workerSkins.Count)
                workerRight.GetComponent<Image>().sprite = workerSkins[0].front;
            else
                workerRight.GetComponent<Image>().sprite = workerSkins[selectedSkin + 1].front;

            //left skin
            if (selectedSkin - 1 < 0)
                workerLeft.GetComponent<Image>().sprite = workerSkins[workerSkins.Count - 1].front;
            else
                workerLeft.GetComponent<Image>().sprite = workerSkins[selectedSkin - 1].front;

        }
    }

    public int GetSelectedSkin()
    {
        return selectedSkin;
    }
}
