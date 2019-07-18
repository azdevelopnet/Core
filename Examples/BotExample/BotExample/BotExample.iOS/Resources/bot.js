function init(token, username) {

   var _directLine = window.WebChat.createDirectLine({ token });
        window.WebChat.renderWebChat({
          directLine: _directLine,
          userID: username
        }, document.getElementById('bot'));


     _directLine.postActivity({ type: 'event', value: username, from: { id: username }, name: 'username' }).subscribe(
        id => console.log("Posted activity", id),
        error => alert(error)
    );
}
