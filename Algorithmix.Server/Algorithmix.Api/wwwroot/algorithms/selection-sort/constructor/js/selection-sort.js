const blockSort = document.querySelector(".blockSort");
let duration = 500;

btnGenerate.onclick = function generateNumbers() {
   while (blockSort.firstChild) {
      blockSort.removeChild(blockSort.firstChild);
   }
   const Numbers = [];
   for (let i = 0; i < 10; i++) {
      const value = Math.floor(Math.random() * 100);
      Numbers.push(value);
   }
   draw(Numbers);
}

btnAdd.onclick = function addNumbers() {
   while (blockSort.firstChild) {
      blockSort.removeChild(blockSort.firstChild);
   }
   let isValid = true;
   const Numbers = NumbersInput.value.split(' ').map(n => {
      const value = parseInt(n);
      if (!(value >= 0 && value < 100)) {
         isValid = false;
      }
      return value;
   });
   if (isValid) {
      draw(Numbers);
   } else {
      alert('Числа должны быть от 0 до 99');
   }
}

NumbersInput.onkeydown = function (e) {
   return e.keyCode >= 48 && e.keyCode <= 57 || e.keyCode === 32 || e.keyCode === 8;
}

function draw(numbers) {
   btnSort.disabled = "";
   for (let i = 0; i < numbers.length; i++) {
      const blockNumber = document.createElement("div");
      blockNumber.classList.add("element");
      blockNumber.style.transform = `translateX(${(i - 4.5) * 75}px)`;

      blockNumber.innerHTML = numbers[i];
      blockSort.appendChild(blockNumber);
   }
}

function swap(element1, element2) {
   return new Promise(resolve => {
      const style1 = window.getComputedStyle(element1);
      const style2 = window.getComputedStyle(element2);


      const transform1 = style1.getPropertyValue("transform");
      const transform2 = style2.getPropertyValue("transform");

      element1.style.transform = transform2;
      element2.style.transform = transform1;

      window.requestAnimationFrame(function () {
         setTimeout(() => {
            var clonedElement1 = element1.cloneNode(true);
            var clonedElement2 = element2.cloneNode(true);

            element2.parentNode.replaceChild(clonedElement1, element2);
            element1.parentNode.replaceChild(clonedElement2, element1);
            clonedElement1.style.backgroundColor = "#b2cfff";
            resolve();
         }, 2000);
      });
   });
}

btnSort.onclick = async function selectionSort() {
   btnSort.disabled = "disabled";
   btnGenerate.disabled = "disabled";
   btnAdd.disabled = "disabled";
   let elements = document.querySelectorAll(".element");
   if (elements.length > 2 && elements.length <= 10) {
      for (let i = 0; i < elements.length - 1; i++) {
         let min = i;
         elements[i].style.backgroundColor = "#FF0000";
         for (let j = i + 1; j < elements.length; j++) {
            elements[j].style.backgroundColor = "#FFD800";

            await new Promise(resolve =>
               setTimeout(() => {
                  resolve();
               }, duration)
            );

            const value1 = Number(elements[j].innerHTML);
            const value2 = Number(elements[min].innerHTML);

            if (value1 < value2) {
               elements[min].style.backgroundColor = "#b2cfff";
               elements[j].style.backgroundColor = "#FF0000";
               min = j;
            } else { elements[j].style.backgroundColor = "#b2cfff"; }

         }
         elements[i].style.backgroundColor = "#FF0000";
         if (i != min) {
            await new Promise(resolve =>
               setTimeout(() => {
                  resolve();
               }, duration)
            );
            await swap(elements[i], elements[min]);
         }
         elements = document.querySelectorAll(".element");
         elements[i].style.backgroundColor = "#00DD21";
      }
      elements[elements.length - 1].style.backgroundColor = "#00DD21";
   } else {
      alert("Пожалуйста, введите от 2 до 10 чисел или сгенерируйте их");
   }
}

$('#animationSpeed').slider({
   formatter: function (value) {
      duration = 1000 * (1 - value);
      return "";
   },
   tooltip: "hide"
});
