import { NgDompurifySanitizer } from "@tinkoff/ng-dompurify";
import { TuiRootModule, TuiDialogModule, TuiAlertModule, TUI_SANITIZER } from "@taiga-ui/core";
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeroesComponent  } from './heroes/heroes.component';
import { ChildComponent } from './child/child.component';
import { FirstpageComponent } from "./firstpage/firstpage.component";
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeroesComponent, ChildComponent, TuiRootModule, TuiDialogModule, TuiAlertModule, FirstpageComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
    providers: [{provide: TUI_SANITIZER, useClass: NgDompurifySanitizer}]
})
export class AppComponent {
  title = 'Code Guru';
}
