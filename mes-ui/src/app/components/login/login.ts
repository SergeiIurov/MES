import {Component} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {AuthService} from '../../services/auth-service';
import {Router} from '@angular/router';
import {jwtDecode} from 'jwt-decode';
import {ButtonDirective} from 'primeng/button';
import {AutoFocus} from 'primeng/autofocus';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    ButtonDirective,
    AutoFocus
  ],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class Login {
  loginForm: FormGroup;
  msg = ''
  isLoginCorrect: boolean = true;

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.loginForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  login() {
    const {username, password} = this.loginForm.value;
    this.authService.login(username, password).subscribe(token => {
      this.authService.token = token;
      const decodedToken = jwtDecode(token);
      this.authService.name = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      this.authService.role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']

      localStorage.setItem('access_token', token);
      if (this.authService.isAuthenticated) {
        this.router.navigate(['/']);
      }
    }, error => {

      const err = JSON.parse(error.error);
      if (err.status === 401) {
        this.msg = "Неверное имя пользователя или пароль.";
      } else {
        this.msg = JSON.parse(error.error).message;
      }
      this.isLoginCorrect = false;
    });
  }

  enterOnlyRead(e) {
    this.loginForm.reset();
    this.authService.login('user', 'user').subscribe(token => {
      e.preventDefault();
      this.authService.token = token;
      const decodedToken = jwtDecode(token);
      this.authService.name = decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name']
      this.authService.role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']

      localStorage.setItem('access_token', token);
      if (this.authService.isAuthenticated) {
        this.router.navigate(['/']);
      }
    });
  }
}
