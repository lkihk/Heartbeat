using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Step_Generator : MonoBehaviour
{

    public GameObject leftArrow;
    public GameObject downArrow;
    public GameObject upArrow;
    public GameObject rightArrow;

    public GameObject leftArrowBack;
    public GameObject downArrowBack;
    public GameObject upArrowBack;
    public GameObject rightArrowBack;

    public float arrowSpeed = 0;

    private bool isInit = false;
    private Song_Parser.Metadata songData;
    private float songTimer = 0.0f;
    private float barTime = 0.0f;
    private float barExecutedTime = 0.0f;
    private GameObject heart;
    private AudioSource heartAudio;
    private Song_Parser.difficulties difficulty;
    private Song_Parser.NoteData noteData;
    private float distance;
    private float originalDistance = 1.0f;
    private float originalArrowSpeed = 0;
    private int barCount = 0;

    private Animator leftAnim;
    private Animator downAnim;
    private Animator upAnim;
    private Animator rightAnim;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Step_Generator.Start()");
        heart = GameObject.FindGameObjectWithTag("Player");
        heartAudio = heart.GetComponent<AudioSource>();

        leftAnim = leftArrowBack.GetComponent<Animator>();
        downAnim = downArrowBack.GetComponent<Animator>();
        upAnim = upArrowBack.GetComponent<Animator>();
        rightAnim = rightArrowBack.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("Step_Generator.Update() - A");

        if (leftAnim.GetBool("isLit"))
        {
            leftAnim.SetBool("isLit", false);
        }
        if (downAnim.GetBool("isLit"))
        {
            downAnim.SetBool("isLit", false);
        }
        if (rightAnim.GetBool("isLit"))
        {
            rightAnim.SetBool("isLit", false);
        }
        if (upAnim.GetBool("isLit"))
        {
            upAnim.SetBool("isLit", false);
        }

        Debug.Log("Step_Generator.Update() - B");



        Debug.Log("Step_Generator.Update() - B isInit:" + isInit);
        Debug.Log("Step_Generator.Update() - B barCount: " + barCount);
        Debug.Log("Step_Generator.Update() - B noteData: " + noteData);
        //Debug.Log("Step_Generator.Update() - B noteData.bars.Count:" + noteData.bars.Count);

        if (isInit && (barCount < noteData.bars.Count))
        {
            Debug.Log("Step_Generator.Update() - B - if");
            float pitch = heartAudio.pitch;
            arrowSpeed = originalArrowSpeed * pitch;

            distance = originalDistance;
            float timeOffset = distance * arrowSpeed;
            songTimer = heartAudio.time;

            if (songTimer - timeOffset >= (barExecutedTime - barTime))
            {
                StartCoroutine(PlaceBar(noteData.bars[barCount++]));

                barExecutedTime += barTime;
            }
        }

        Debug.Log("Step_Generator.Update() - C");
    }

    IEnumerator PlaceBar(List<Song_Parser.Notes> bar)
    {
        for (int i = 0; i < bar.Count; i++)
        {
            if (bar[i].left)
            {
                GameObject obj = (GameObject)Instantiate(leftArrow, new Vector3(leftArrowBack.transform.position.x, leftArrowBack.transform.position.y + distance, leftArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = leftArrowBack;
            }
            if (bar[i].down)
            {
                GameObject obj = (GameObject)Instantiate(downArrow, new Vector3(downArrowBack.transform.position.x, downArrowBack.transform.position.y + distance, downArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = downArrowBack;
            }
            if (bar[i].up)
            {
                GameObject obj = (GameObject)Instantiate(upArrow, new Vector3(upArrowBack.transform.position.x, upArrowBack.transform.position.y + distance, upArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = upArrowBack;
            }
            if (bar[i].right)
            {
                GameObject obj = (GameObject)Instantiate(rightArrow, new Vector3(rightArrowBack.transform.position.x, rightArrowBack.transform.position.y + distance, rightArrowBack.transform.position.z - 0.3f), Quaternion.identity);
                obj.GetComponent<Arrow_Movement>().arrowBack = rightArrowBack;
            }
            yield return new WaitForSeconds((barTime / bar.Count) - Time.deltaTime);
        }
    }

    public void InitSteps(Song_Parser.Metadata newSongData, Song_Parser.difficulties newDifficulty)
    {
        Debug.Log("Step_Generator.InitSteps()");

        songData = newSongData;
        isInit = true;
        barTime = (60.0f / songData.bpm) * 4.0f;
        difficulty = newDifficulty;
        distance = originalDistance;




        switch (difficulty)
        {
            case Song_Parser.difficulties.beginner:
                if (songData.beginnerExists)
                {
                    Debug.Log("Step_Generator.InitSteps() - beginner");
                    arrowSpeed = 0.007f;
                    noteData = songData.beginner;
                }
                else
                {
                    goto case Song_Parser.difficulties.easy;
                }
                break;
            case Song_Parser.difficulties.easy:
                if (songData.easyExists)
                {
                    Debug.Log("Step_Generator.InitSteps() - easy");
                    arrowSpeed = 0.009f;
                    noteData = songData.easy;
                }
                else
                {
                    goto case Song_Parser.difficulties.medium;
                }
                break;
            case Song_Parser.difficulties.medium:
                if (songData.mediumExists)
                {
                    Debug.Log("Step_Generator.InitSteps() - medium");
                    arrowSpeed = 0.011f;
                    noteData = songData.medium;
                }
                else
                {
                    goto case Song_Parser.difficulties.hard;
                }
                break;
            case Song_Parser.difficulties.hard:
                if (songData.hardExists)
                {
                    Debug.Log("Step_Generator.InitSteps() - hard");
                    arrowSpeed = 0.013f;
                    noteData = songData.hard;
                }
                else
                {
                    goto case Song_Parser.difficulties.challenge;
                }
                break;
            case Song_Parser.difficulties.challenge:
                if (songData.challengeExists)
                {
                    Debug.Log("Step_Generator.InitSteps() - challenge");
                    originalArrowSpeed = 0.009f;
                    arrowSpeed = 0.016f;
                    noteData = songData.challenge;
                }
                else
                {
                    goto case Song_Parser.difficulties.beginner;
                }
                break;
            default:
                Debug.Log("Step_Generator.InitSteps() - default");
                goto case Song_Parser.difficulties.beginner;
        }

        originalArrowSpeed = arrowSpeed;
    }
}
