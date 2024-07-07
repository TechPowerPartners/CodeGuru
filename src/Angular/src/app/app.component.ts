import { Component } from '@angular/core';
import { NgDompurifySanitizer } from "@tinkoff/ng-dompurify";
import { TuiRootModule, TuiDialogModule, TuiAlertModule, TUI_SANITIZER } from "@taiga-ui/core";
import { RouterLink, RouterOutlet } from '@angular/router';
import { HeroesComponent  } from './heroes/heroes.component';
import { FirstpageComponent } from "./firstpage/firstpage.component";
import { FooterComponent } from "./footer/footer.component";
import { TrainingComponent } from "./training/training.component";
import { ArticlesComponent } from './articles/articles.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeroesComponent, TuiRootModule, TuiDialogModule, TuiAlertModule, FirstpageComponent, FooterComponent, TrainingComponent, RouterLink, ArticlesComponent ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [{provide: TUI_SANITIZER, useClass: NgDompurifySanitizer}],
})

export class AppComponent {
  title = 'Code Guru';
}
