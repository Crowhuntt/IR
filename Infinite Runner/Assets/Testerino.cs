using UnityEngine;
using System.Collections;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Testerino : MonoBehaviour {
    private string logedUserEmail = "lol@lol.com";
    private string logedPass = "lollol1";
    private string logedUserName = "lol";
    private string uID = "ms326WHabAhrRF7N4l018BCkzb53";
    private float scorePartida = 8;
    //private float scoredb;

    void Start()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        //CreateFuckingUser(auth);

        //Elijo la base de datos Firebase
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://infinite-runner-e4e8b.firebaseio.com/");

        ComprobarUsuarioExiste();
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
    
    private void ActualizarRanking() {
        //Coger todos los valores de GetReference("score").Child(“scoreID”).OrderByChild("score") y ordenarlos descending.OrderByChild()
        //Leerlos descending en BUCLE y asignarle su nueva posición y en la posición asignar el scoreID.
        //while x.lenght => 
        //    snapshotScoreID.Child(“pos”).Value = x
        //    var scoreID = snapshotScoreID.Value; ?
	    //    snapshotPosition.Child(x).Child(“scoreID”).SetValueAsync(scoreID);
        //end bucle.
    }

    private void CreateFuckingUser(FirebaseAuth auth){
        auth.CreateUserWithEmailAndPasswordAsync(logedUserEmail, logedPass).ContinueWith(task =>
         {
         
            FirebaseUser kektusUser = task.Result;
             auth.SignOut();
         });
    }
}
