using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance { get; private set; }
    [SerializeField] private ProgresBarDrawer _progressBar;

    private AsyncOperation _asyncOperation;
    private Canvas _canvas;
    [SerializeField] private float _speed;
    [SerializeField] private bool _loadingDone=false;
    [SerializeField] private float _progress=0;
    private float _time;
    private SceneSetter _sceneSetter;
   

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
            instance = this;
        _canvas =GetComponentInChildren<Canvas>(true);
        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(int index)
    {
        _sceneSetter = FindObjectOfType<SceneSetter>();
        _sceneSetter.SceneUnload();
        Time.timeScale = 0;
        _loadingDone =false;
        _progress = 0;
        _canvas.gameObject.SetActive(true);
        StartCoroutine(BeginLoad(index));
    }
    private void Update()
    {
        
        if (_progress != 100)
        {
            _time += _speed *Time.unscaledDeltaTime;
            _progress = Mathf.Lerp(0, 100,  _time);
            UpdateProgressUI(_progress);
        }
        else if(_progress == 100 && _loadingDone)
        {
            _sceneSetter=FindObjectOfType<SceneSetter>();
            _sceneSetter.SceneLoaded();
            Time.timeScale = 1;
            _time = 0;
            _canvas.gameObject.SetActive(false);
        }
        
    }
    private IEnumerator BeginLoad(int index)
    {
        _asyncOperation = SceneManager.LoadSceneAsync(index);
        while (!_asyncOperation.isDone)
        {

            yield return null;
        }
        _asyncOperation = null;
        _loadingDone = true;
    }
    private void UpdateProgressUI(float progress)
    {
        _progressBar.CurrentValue = progress;
    }
}
