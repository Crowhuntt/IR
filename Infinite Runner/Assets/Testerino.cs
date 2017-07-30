using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Testerino : MonoBehaviour {
    private string logedUserEmail = "lol5@lol.com";
    private string logedPass = "lollol5";
    private string logedUserName = "lol5";
    private string uID = "szH9wgf7BjT3q0PovExZc5c0Wo33";
    //private string uID;
    private float scorePartida = 10;
    //private float scoredb;

    void Start()
    {
        //Elijo la base de datos Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://infinite-runner-e4e8b.firebaseio.com/");

        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();

        ActualizarRanking();
        //GetInfoUsuario(auth);

        //CrearUsuario(auth);
        //IniciarSesion(auth);
        //GetUserInfo(auth);
        //InsertarUsuarioBBDD(uID, logedUserName, logedUserEmail);

        //ComprobarUsuarioExiste();
    }

    //RECOGER LA INFO DEL USUARIO LOGUEADO PARA DESPUES COMPROBAR SI EXISTE EN LA BBDD
    private void GetUserInfo(FirebaseAuth auth)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            //No usamos DisplayName de momento
            string name = user.DisplayName;
            //Recoge el email del usuario logueado
            string email = user.Email;
            //Recoge el uID del usuario logueado
            uID = user.UserId;

            ComprobarUsuarioExiste(uID);
        }
        else {
            Debug.Log("Usuario no existe o no logueado");
        }
    }

    //COMPROBAR SI EL USUARIO EXISTE EN LA BASE DE DATOS
    private void ComprobarUsuarioExiste(string uID) {
        DatabaseReference users = FirebaseDatabase.DefaultInstance.GetReference("users");
        users.Child(uID).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                //Algo he hecho mal cuando se ha dado de alta el usuario, debería existir siempre 
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshotUsers = task.Result;
                ComprobarPuntuacionMaxima(snapshotUsers.Child("scoreID").Value.ToString());
                //Mirar si el usuario ya existe en la base de datos
                if (snapshotUsers.HasChild(uID)) {
                    //ComprobarPuntuacionMaxima(snapshotUsers.Child(uID).Child("scoreID").Value.ToString());
                }
                else {
                    //Algo he hecho mal cuando se ha dado de alta el usuario, debería existir siempre 
                }
            }
        });
    }

    //COMPROBAR SI LA PUNTUACIÓN OBTENIDA ES LA MAYOR DEL USUARIO LOGUEADO
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
        //LLAMAR A FUNCION PARA ACTUALIZAR EL RANKING
        ActualizarRanking();
    }
    
    //ACTUALIZA EL RANKING
    private void ActualizarRanking() {
        DatabaseReference users = FirebaseDatabase.DefaultInstance.GetReference("users");
        DatabaseReference scores = FirebaseDatabase.DefaultInstance.GetReference("scores");
        DatabaseReference position = FirebaseDatabase.DefaultInstance.GetReference("position");
        float contadorPosicion = 0;

        //Coger todos los "score" y los ordeno ASCENDING (es lo unico que deja firebase)
        //TRUCO: Las puntuaciones las guardo en negativo, por lo que saldran primero los de mejor puntuacion
        scores.OrderByChild("score").GetValueAsync().ContinueWith(task => {
            //Si quisiese coger solo los X primeros utilizo scores.OrderByChild("score").LimitToFirst(10).GetValueAsync()...
            if (task.IsFaulted) {
                // Handle the error...
            }
            else if (task.IsCompleted) {
                //Este diccionario "snapshot" es "scores" y contiene todos los scoreID
                DataSnapshot snapshot = task.Result;
                //Este diccionario "snapshotScoreID" contiene todos los datos de cada scoreID
                foreach (DataSnapshot snapshotScoreID in snapshot.Children)
                {                                   
                    //Value del score (Puntuacion) INVERTIDO a positivo
                    float puntuacion = - float.Parse(snapshotScoreID.Child("score").Value.ToString());
                    //Debug.Log(puntuacion);

                    //HACER MODIFICACIONES SOBRE BBDD
                    contadorPosicion = contadorPosicion + 1;
                    //Cojo el scoreID del registro actual
                    string scoreID = snapshotScoreID.Key;
                    //Modifico registros en BBDD
                    position.Child(contadorPosicion.ToString()).Child("scoreID").SetValueAsync(scoreID);
                    scores.Child(scoreID).Child("position").SetValueAsync(contadorPosicion.ToString());
                }
            }
        });
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

    //RECOGER LA INFORMACION DEL USUARIO - NO ESTA EN USO DE MOMENTO!!!!!!!
    private void GetInfoUsuario(FirebaseAuth auth) {
        //FirebaseUser user = auth.CurrentUser;
        //if (user != null)
        //{
            //No usamos DisplayName de momento
            //string name = user.DisplayName;
            string name = "lol5";
            //Recoge el email del usuario logueado
            //string email = user.Email;
            string email = "lol5@lol.com";
            //Recoge el uID del usuario logueado
            //uID = user.UserId;
            uID = "szH9wgf7BjT3q0PovExZc5c0Wo33";

        InsertarUsuarioBBDD(uID, name, email);
            //ComprobarUsuarioExiste();
        //}
        //else
        //{
        //    Debug.Log("Usuario no existe o no logueado");
        //}
    }

    //INSERTAR NUEVO USUARIO A LA BASE DE DATOS
    private void InsertarUsuarioBBDD(string uID, string name, string email)
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
                scores.Child(scoreID).Child("score").SetValueAsync(0);
                scores.Child(scoreID).Child("userID").SetValueAsync(uID);
            }
        });  
    }


    //INICIAR SESION - No parece funcionar desde PC?
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

                //GetUserInfo(auth);
                //InsertarUsuarioBBDD(uID, logedUserName, logedUserEmail);
            }
        });
    }

    

}
