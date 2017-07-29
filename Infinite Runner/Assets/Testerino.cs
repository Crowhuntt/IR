using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Testerino : MonoBehaviour {
    private string logedUserEmail = "lol4@lol.com";
    private string logedPass = "lollol4";
    private string logedUserName = "lol4";
    //private string uID = "ms326WHabAhrRF7N4l018BCkzb53";
    private string uID;
    private float scorePartida = 101;
    //private float scoredb;

    void Start()
    {
        //Elijo la base de datos Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://infinite-runner-e4e8b.firebaseio.com/");

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
        //CrearUsuario(auth);
        //IniciarSesion(auth);
        //GetUserInfo(auth);
        InsertarUsuarioBBDD(auth);

        //ComprobarUsuarioExiste();
    }

    private void ComprobarUsuarioExiste() {
        DatabaseReference users = FirebaseDatabase.DefaultInstance.GetReference("users");
        users.GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                // Handle the error...
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshotUsers = task.Result;
                //Mirar si el usuario ya existe en la base de datos
                if (snapshotUsers.HasChild(uID)) {
                    ComprobarPuntuacionMaxima(snapshotUsers.Child(uID).Child("scoreID").Value.ToString());
                }
                else {
                    //Algo he hecho mal cuando se ha dado de alta el usuario, debería existir siempre 
                }
            }
        });
    }

    private void ComprobarPuntuacionMaxima(string scoreID) {
        DatabaseReference scores = FirebaseDatabase.DefaultInstance.GetReference("scores");
        //float scoreBBDD;
        scores.Child(scoreID).GetValueAsync().ContinueWith(task2 => {
            if (task2.IsFaulted) {
                // Handle the error...
            }
            else if (task2.IsCompleted) {
                DataSnapshot snapshotScores = task2.Result;
                //Mirar si la puntuacion de la BBDD es menor a la obtenida
                if (float.Parse(snapshotScores.Child("score").Value.ToString()) < scorePartida){
                    InsertarPuntuacionMaxima(scoreID);
                }
                else{
                    //No actualizar porque la puntuación obtenida es menor al record personal 
                }
            }
        });
    }

    private void InsertarPuntuacionMaxima(string scoreID) {
        DatabaseReference scores = FirebaseDatabase.DefaultInstance.GetReference("scores");
        scores.Child(scoreID).Child("score").SetValueAsync(scorePartida);
        scores.Child(scoreID).Child("date").SetValueAsync(System.DateTime.Now.ToString("dd/MM/yyyy"));
        //LLAMAR A FUNCION PARA ACTUALIZAR EL RANKING "position"
        ActualizarRanking();
    }
    
    //ACTUALIZA EL RANKING
    private void ActualizarRanking() {
        //Coger todos los valores de GetReference("score").Child(“scoreID”).OrderByChild("score") y ordenarlos descending.OrderByChild()
        //Leerlos descending en BUCLE y asignarle su nueva posición y en la posición asignar el scoreID.
        //while x.lenght => 
        //    snapshotScoreID.Child(“pos”).Value = x
        //    var scoreID = snapshotScoreID.Value; ?
	    //    snapshotPosition.Child(x).Child(“scoreID”).SetValueAsync(scoreID);
        //end bucle.
    }

    //CREACION DE USUARIO EN AUTH
    private void CrearUsuario(FirebaseAuth auth){
        auth.CreateUserWithEmailAndPasswordAsync(logedUserEmail, logedPass).ContinueWith(task =>
         {
             if (task.IsCanceled)
             {
                 Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                 return;
             }
             if (task.IsFaulted)
             {
                 Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                 return;
             }
             if (task.IsCompleted) {
                 //Debug.Log("CREADOOOOO");
                 FirebaseUser newUser = task.Result;
                 Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);

             }
         });
    }

    //INSERTAR USUARIO EN LA BASE DE DATOS DESPUES DE CREARLO EN AUTH
    private void InsertarUsuarioBBDD(FirebaseAuth auth) {
        //FirebaseUser user = auth.CurrentUser;
        //if (user != null)
        //{
            //No usamos DisplayName de momento
            //string name = user.DisplayName;
            string name = "Sin nombre";
            //Recoge el email del usuario logueado
            //string email = user.Email;
            string email = "a@a.com";
            //Recoge el uID del usuario logueado
            //uID = user.UserId;
            uID = "ABCDEFG";

            WriteNewUser(uID, name, email);
            //ComprobarUsuarioExiste();
        //}
        //else
        //{
        //    Debug.Log("Usuario no existe o no logueado");
        //}
    }

    private void WriteNewUser(string uID, string name, string email)
    {
        DatabaseReference users = FirebaseDatabase.DefaultInstance.GetReference("users");
        DatabaseReference scores = FirebaseDatabase.DefaultInstance.GetReference("scores");
        DatabaseReference position = FirebaseDatabase.DefaultInstance.GetReference("position");
        float count;

        //USERS
        //Genero un scoreID unique
        string scoreID = scores.Push().Key;
        //Añado el usuario a la BBDD        
        users.Child(uID).Child("name").SetValueAsync(name);
        users.Child(uID).Child("email").SetValueAsync(email);
        users.Child(uID).Child("scoreID").SetValueAsync(scoreID);

        //POSITION
        //COUNT CUANTAS POSICIONES HAY PARA ASIGNARLE LA ULTIMA A LOS NUEVOS USUARIOS CREADOS
        position.GetValueAsync().ContinueWith(task2 => {
            if (task2.IsFaulted) {
                // Handle the error...
            }
            else if (task2.IsCompleted) {
                DataSnapshot snapshot = task2.Result;
                //Cuento cuantos hijos hay (numero de posiciones totales) y añado una, que sera la que queremos crear
                count = float.Parse(snapshot.ChildrenCount.ToString()) + 1;
                //Añadimos a la BBDD la ultima posicion con el scoreID del usuario
                position.Child(count.ToString()).Child("scoreID").SetValueAsync(scoreID);

                //SCORES
                scores.Child(scoreID).Child("date").SetValueAsync(System.DateTime.Now.ToString("dd/MM/yyyy"));
                scores.Child(scoreID).Child("position").SetValueAsync(count.ToString());
                scores.Child(scoreID).Child("score").SetValueAsync("0");
                scores.Child(scoreID).Child("userID").SetValueAsync(uID);
            }
        });  
    }


    //INICIAR SESION
    private void IniciarSesion(FirebaseAuth auth) {
        auth.SignInWithEmailAndPasswordAsync(logedUserEmail, logedPass).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted)  {
                //Debug.Log("LOGUEADOOOO");
                FirebaseUser newUser = task.Result;
                //Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
                GetUserInfo(auth);
            }
        });
    }

    //De momento no necesito para nada esta function
    private void GetUserInfo(FirebaseAuth auth) {
        FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            //No usamos DisplayName de momento
            string name = user.DisplayName;
            //Recoge el email del usuario logueado
            string email = user.Email;
            //Recoge el uID del usuario logueado
            uID = user.UserId;

            //ComprobarUsuarioExiste();
        }
        else {
            Debug.Log("Usuario no existe o no logueado");
        }
    }

}
