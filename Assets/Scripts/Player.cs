using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Player : MonoBehaviour
{
    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject _plantPrefab;
    [SerializeField] private int _numSeeds = 5; 
    [SerializeField] private PlantCountUI _plantCountUI;

    private int _numSeedsLeft;
    private int _numSeedsPlanted;

    private void Start ()
    {
        _numSeedsLeft = _numSeeds; //This is here because you START the game with a limited numebr of seeds to plant.
        _numSeedsPlanted = 0;
        _plantCountUI.UpdateSeeds(_numSeedsLeft, _numSeedsPlanted);
    }

    private void Update()
    {
        //Steps for basic movement: First, read input. Second, convert input into (vector) direction. Third, move player.
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        //"Raw" outputs an instant response and has no smoothing. First step done. Move on to second.
        Vector2 direction = new Vector2(horizontal, vertical);
        //Second step done. Move on to third.
        transform.position += (Vector3)(direction * _speed * Time.deltaTime);
        //Third step done. "transform.position" is safer to top-down games. It makes it so the movement is relative to the object’s world space. "transform.Translate" makes movement relative to the object's rotation. Transform only accepts vector3. Movement (ex: direction * _speed * Time.deltaTime) is still vector2. Vector3 is just the delivery mechanism. It's like writing a letter (Vector2) and putting it in an envelope (Vector3) so the mail system accepts it.

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlantSeed();
        }
    }

    public void PlantSeed ()
    {
        if (_numSeedsLeft <= 0)
        {
            return;
        }
        Vector3 plantPosition = transform.position; //Vector2 (describes 2d motion) is better for movement. Vector3 (applies that motion to Unity's Transform System) is better for placement. 
        Instantiate(_plantPrefab, plantPosition, Quaternion.identity); //_plantPrefab is the blueprint. plantPosition is the where the plant will appear. Quaternion.identity makes it so the plant has no rotation.
        _numSeedsLeft--; //Computes first
        _numSeedsPlanted++;
        _plantCountUI.UpdateSeeds(_numSeedsLeft, _numSeedsPlanted); //Then displays
    }
    //Use Vector2 to describe motion and intent in 2D. Use Vector3 only when interacting with Transform.position or world placement.
}
