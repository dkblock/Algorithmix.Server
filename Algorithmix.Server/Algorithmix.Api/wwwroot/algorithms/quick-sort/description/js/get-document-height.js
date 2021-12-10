window.addEventListener('message', event => {
   const documentHeight = document.body.scrollHeight;
   event.source.postMessage(documentHeight, "*");
});

