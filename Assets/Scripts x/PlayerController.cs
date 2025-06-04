using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PlayerController : MonoBehaviour
{
    public DynamicJoystick joystick;
    public float speed;
    public int speedRotating;
    public Animator anim;
    public Rigidbody rb;
    public Transform mainChar;


    Vector3 dirSmooth = Vector3.zero;

    public CharacterSc character;

    void FixedUpdate()
    {
        if (character.dead || !character.move) return;

        Vector3 direction = Camera.main.transform.forward * joystick.Vertical + Camera.main.transform.right * joystick.Horizontal;
        direction = new Vector3(direction.x, 0, direction.z);
        dirSmooth = Vector3.Lerp(dirSmooth, direction, Time.fixedDeltaTime * 7);

        //transform.Translate(direction * speed * Time.deltaTime);
        //rb.velocity = direction * speed * 100 * Time.deltaTime;

        rb.MovePosition(rb.position + dirSmooth * speed * Time.deltaTime);

        anim.SetFloat("Movement", dirSmooth.magnitude);

        if (direction != Vector3.zero) mainChar.rotation = Quaternion.Lerp(mainChar.rotation, Quaternion.LookRotation(direction), 10 * Time.fixedDeltaTime);

    }
}
