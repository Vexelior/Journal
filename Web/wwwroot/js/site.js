document.addEventListener('DOMContentLoaded', function () {
    var submitButton = this.querySelectorAll('[type="submit"]');
    if (!submitButton || submitButton.length === 0) {
        return;
    }
    submitButton.forEach(function (button) {
        button.addEventListener('click', function () {
            var buttonWidth = this.offsetWidth;
            this.style.width = buttonWidth + 'px';
            var buttonHeight = this.offsetHeight;
            this.style.height = buttonHeight + 'px';
            var form = this.closest('form:not(#logoutForm)');
            if (form) {
                if (form.reportValidity()) {
                    this.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span>';
                    this.disabled = true;
                    this.form.submit();
                }
            }
        });
    });

    $('#summernote').summernote({
        height: 300,
        placeholder: 'Write your journal entry here...',
        toolbar: [
            ['style', ['style']],
            ['font', ['bold', 'underline', 'clear']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['table', ['table']],
            ['insert', ['link']],
            ['view', ['fullscreen', 'codeview', 'help']]
        ]
    });
    document.querySelectorAll('.dropdown-submenu > .dropdown-toggle').forEach(function (element) {
        element.addEventListener('click', function (e) {
            e.preventDefault();
            e.stopPropagation();
            var submenu = this.nextElementSibling;
            if (submenu) {
                submenu.classList.toggle('show');
            }
        });
    });
    if (window.matchMedia('(min-width: 768px)').matches) {
        document.querySelectorAll('.dropdown-submenu').forEach(function (element) {
            element.addEventListener('mouseenter', function () {
                var submenu = this.querySelector('.dropdown-menu');
                if (submenu) {
                    submenu.classList.add('show');
                }
            });
            element.addEventListener('mouseleave', function () {
                var submenu = this.querySelector('.dropdown-menu');
                if (submenu) {
                    submenu.classList.remove('show');
                }
            });
        });
        document.querySelectorAll('.dropdown').forEach(function (element) {
            element.addEventListener('mouseenter', function () {
                var menu = this.querySelector('.dropdown-menu');
                if (menu) {
                    menu.classList.add('show');
                }
            });

            element.addEventListener('mouseleave', function () {
                var menu = this.querySelector('.dropdown-menu');
                if (menu) {
                    menu.classList.remove('show');
                }
            });
        });
    }
});