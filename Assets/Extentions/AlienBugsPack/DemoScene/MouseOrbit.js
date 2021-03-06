var target : Transform;
var distance = 10.0;

var xSpeed = 250.0;
var ySpeed = 120.0;

var yMinLimit = -20;
var yMaxLimit = 80;

var zoomSpeed = 10;

private var x = 0.0;
private var y = 0.0;

@script AddComponentMenu("Camera-Control/Mouse Orbit")

function Start () {
    var angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;

	// Make the rigid body not change rotation
   	if (GetComponent.<Rigidbody>())
		GetComponent.<Rigidbody>().freezeRotation = true;
}

function LateUpdate () {
    
    //Zooming
    if (Input.GetMouseButton(2))
        	distance =distance + Input.GetAxis("Mouse Y") * zoomSpeed * 0.02;
    
    if(Input.GetKey("a")||Input.GetAxis("Mouse ScrollWheel") > 0)
    	distance =distance - zoomSpeed * 0.02;
    if(Input.GetKey("z")||Input.GetAxis("Mouse ScrollWheel") < 0)
    	distance =distance + zoomSpeed * 0.02;
    

    //Navigation
    if (Input.GetMouseButton(0))
    {
     	x += Input.GetAxis("Mouse X") * xSpeed * 0.02;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02;
 		y = ClampAngle(y, yMinLimit, yMaxLimit);
    }
	
	if(Input.GetKey("up")||Input.GetKey("down"))
	{
		y += Input.GetAxis("Vertical") * ySpeed * 0.02;
		y = ClampAngle(y, yMinLimit, yMaxLimit);
	}
	if((Input.GetKey("left")||Input.GetKey("right")))
	{
		x -= Input.GetAxis("Horizontal") * xSpeed * 0.02;
	}
	
   
    if (target) {      
        var rotation = Quaternion.Euler(y, x, 0);
        var position = rotation * Vector3(0.0, 0.0, -distance) + target.position; 
        transform.rotation = rotation;
        transform.position = position;
    } 
}

static function ClampAngle (angle : float, min : float, max : float) {
	if (angle < -360)
		angle += 360;
	if (angle > 360)
		angle -= 360;
	return Mathf.Clamp (angle, min, max);
}