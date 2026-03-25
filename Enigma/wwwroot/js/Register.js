const slider = document.querySelector('.slider');
const slides = slider.querySelectorAll('li');

// Store the total number of images
const slideCount = slides.length;
let activeSlide = 0;

// To change the images dynamically every 
// 5 Secs, use SetInterval() method
setInterval(() => {
    slides[activeSlide].classList.remove('active');
    activeSlide++;
    if (activeSlide === slideCount) {
        activeSlide = 0;
    }
    slides[activeSlide].classList.add('active');
}, 2000);

document.addEventListener('DOMContentLoaded', function () {
    var inputs = document.querySelectorAll('.input100');

    inputs.forEach(function (input) {
        if (input.value) {
            input.classList.add('has-val');
        }

        input.addEventListener('focusout', function () {
            if (input.value) {
                input.classList.add('has-val');
            } else {
                input.classList.remove('has-val');
            }
        });
    });
});