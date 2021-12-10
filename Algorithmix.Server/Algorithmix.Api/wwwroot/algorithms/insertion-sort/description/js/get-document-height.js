window.addEventListener('message', event => {
    const documentHeight = document.body.scrollHeight;
    event.source.postMessage(documentHeight, "*");
});

function insertionSort(arr) {
    for (let j = 1; j < array.length; j++) {
        let current = array[j];
        let i = j-1;
        while ((i > -1) && (array[i] > current)) {
            array[i+1] = array[i];
            i--;
        }
        array[i+1] = current;
    }
}